from http.client import HTTPException

import pandas as pd
import numpy as np
from typing import List, Optional, Dict, Tuple
from mlxtend.preprocessing import TransactionEncoder
from mlxtend.frequent_patterns import fpgrowth, association_rules
from models.pydantic_models import ProductInputModel, OrderDetailInputModel, FrequentItemset, AssociationRule, \
    AnalysisResult, FrequentlyBoughtTogether


def _create_product_map(products: List[ProductInputModel]) -> Dict[str, str]:
    """Creates a mapping from Product ID to Product Name."""
    if not products:
        return {}
    return {p.product_id: p.product_name for p in products}

def _prepare_transactions(order_details: List[OrderDetailInputModel]) -> List[List[str]]:
    """Prepares a list of transactions (list of product IDs per order)."""
    if not order_details:
        return []

    order_details_data = [
        {'order_id': od.order_id, 'product_id': od.product_id}
        for od in order_details
    ]
    if not order_details_data:
        return []

    # Convert to DataFrame for easier manipulation
    df = pd.DataFrame(order_details_data)
    if df.empty or 'order_id' not in df.columns or 'product_id' not in df.columns:
        return []

    # Group by order_id and aggregate product_ids into sets
    transactions_grouped = df.groupby('order_id')['product_id'].apply(set).tolist()
    # Convert sets to lists, filtering out empty sets
    """
    ['P001', 'P002'],  # Giao dịch O001
    ['P003', 'P002'],  # Giao dịch O002
    """
    return [list(itemset) for itemset in transactions_grouped if itemset]

# This function runs the FP-Growth algorithm to find frequent itemsets
def _run_fpgrowth_analysis(
    transactions: List[List[str]],
    min_support: float
) -> Optional[pd.DataFrame]:
    """Runs the FP-Growth algorithm to find frequent itemsets."""
    if not transactions:
        return None
    # Initialize TransactionEncoder and fit to the transactions
    # Ensure transactions are not empty and contain valid data
    # TransactionEncoder is used to convert the list of transactions into a one-hot encoded DataFrame
    # It describe the presence of each product in each transaction

    te = TransactionEncoder()
    try:
        valid_transactions = [t for t in transactions if t]
        if not valid_transactions:
            return None
        # Sử dụng fit_transform để chuyển đổi dữ liệu giao dịch thành một ma trận nhị phân.
        te_ary = te.fit_transform(valid_transactions)
        if te_ary.size == 0:
            return None
    except ValueError:
        print("Error during TransactionEncoder fitting.")
        return None

    #Chuyển đổi ma trận nhị phân thành một DataFrame của pandas. Mỗi cột trong DataFrame đại diện cho một mục trong các giao dịch.
    df_encoded = pd.DataFrame(te_ary, columns=te.columns_)
    if df_encoded.empty:
        return None

    try:
        """
        itemsets        support
    0  (P002)        0.6667
    1  (P001, P002)  0.3333

        """
        frequent_itemsets_df = fpgrowth(df_encoded, min_support=min_support, use_colnames=True)
        return frequent_itemsets_df if not frequent_itemsets_df.empty else None
    except Exception as e:
        print(f"Error during fpgrowth execution: {e}")
        return None


def _generate_association_rules(
    frequent_itemsets_df: pd.DataFrame,
    min_confidence: float
) -> Optional[pd.DataFrame]:
    """Generates association rules from frequent itemsets."""
    if frequent_itemsets_df is None or frequent_itemsets_df.empty:
        return None
    try:
        """
        Output example:
        antecedents  consequents  confidence  lift
        0  (P001)     (P002)       0.80        1.2

        """
        #After use association_rules, we can extract the rules with the specified minimum confidence.
        print("Test frequent item set df: ",frequent_itemsets_df)


        rules_df = association_rules(frequent_itemsets_df, metric="confidence", min_threshold=min_confidence)
        print("test rule df: ", rules_df)
        if rules_df.empty:
            print("No rules found with the given min_confidence.")
        return rules_df
    except Exception as e:
        print(f"Error during association_rules generation: {e}")
        return None


def _format_results(
    frequent_itemsets_df: Optional[pd.DataFrame],
    rules_df: Optional[pd.DataFrame],
    product_map: Dict[str, str,]
) -> Tuple[List[FrequentItemset], List[AssociationRule]]:
    """Formats the DataFrames into Pydantic model lists, mapping IDs to names."""
    output_itemsets = []
    output_rules = []


    #Return a tuple of two functions to map IDs to product names
    def map_ids(ids: frozenset) -> Tuple[int, ...]:
        # Map từ ProductId sang ProductName sử dụng product_map
        return tuple(product_map.get(int(id_val), f"Unknown({id_val})") for id_val in ids)

    # Check if the DataFrames are not None and not empty before processing
    #Chuyen doi thanh froezenset neu can thiet
    if frequent_itemsets_df is not None and not frequent_itemsets_df.empty:
        if 'itemsets' in frequent_itemsets_df.columns and not frequent_itemsets_df.empty:
            if not isinstance(frequent_itemsets_df['itemsets'].iloc[0], frozenset):
                 frequent_itemsets_df['itemsets'] = frequent_itemsets_df['itemsets'].apply(lambda x: frozenset(x) if not isinstance(x, frozenset) else x)

            for _, row in frequent_itemsets_df.iterrows():
                try:
                    output_itemsets.append(FrequentItemset(
                        itemset=row['itemsets'],
                        support=row['support']
                    ))
                    print("Mk khung",output_itemsets[-1])
                except Exception as e:
                    print(f"Error formatting itemset row: {row}. Error: {e}")
                    continue

#Chuyen doi thanh froezenset neu can thiet
    if rules_df is not None and not rules_df.empty:
        required_cols = ['antecedents', 'consequents', 'antecedent support',
                         'consequent support', 'support', 'confidence', 'lift',
                         'leverage', 'conviction']

        if not all(col in rules_df.columns for col in required_cols):
             print("Warning: Missing expected columns in rules DataFrame. Check mlxtend version or results.")
        else:
            rules_df_filtered = rules_df[required_cols].copy()
            rules_df_filtered.rename(columns={
                'antecedent support': 'antecedent_support',
                'consequent support': 'consequent_support'
            }, inplace=True)

            rules_df_filtered.replace([np.inf, -np.inf, np.nan], None, inplace=True)

            if not rules_df_filtered.empty:
                if 'antecedents' in rules_df_filtered.columns and not isinstance(rules_df_filtered['antecedents'].iloc[0], frozenset):
                    rules_df_filtered['antecedents'] = rules_df_filtered['antecedents'].apply(lambda x: frozenset(x) if not isinstance(x, frozenset) else x)
                if 'consequents' in rules_df_filtered.columns and not isinstance(rules_df_filtered['consequents'].iloc[0], frozenset):
                    rules_df_filtered['consequents'] = rules_df_filtered['consequents'].apply(lambda x: frozenset(x) if not isinstance(x, frozenset) else x)


            for row_dict in rules_df_filtered.to_dict('records'):
                try:
                    output_rules.append(AssociationRule(**row_dict))
                except Exception as e:
                    print(f"Error processing rule row: {row_dict}")
                    print(f"Validation/Processing error: {e}")
                    continue

    output_itemsets.sort(key=lambda x: x.support, reverse=True)
    output_rules.sort(key=lambda x: (
        x.confidence is not None,
        x.confidence if x.confidence is not None else -1,
        x.lift is not None,
        x.lift if x.lift is not None else -1
        ), reverse=True)


    return output_itemsets, output_rules


def transform_data(frequent_itemsets, products):
    print("frequent_itemsets:", frequent_itemsets)
    print("products:", products)
    groups = []
    product_map = {product.product_id: product for product in products}
    print("Product map:", product_map)
    group_counter = 1
    for itemset in frequent_itemsets:
        group_name = f"Group #{group_counter}"
        group_items = []
        print("Processing itemset:", itemset)

        # Lấy sản phẩm từ itemset
        for product_id in itemset.itemset:
            print("product_id:", product_id)
            product = product_map.get(product_id)
            print("productA:", product)
            if product:
                print("ABcd")
                group_items.append({
                    'id': product.product_id,
                    'productCode': product.product_code,
                    'name': product.product_name,
                    'categoryName': product.product_category,
                    'price': product.product_price,
                    'stockQuantity': product.product_stock_quantity,
                    'costPrice': product.product_cost_price,
                    'discount': product.product_discount,
                    'imageUrl': product.product_image_path,
                    'categoryId': product.product_category_id,
                })
            print("Product appended: ", product)

        # Số lượng đơn hàng cho nhóm này (giả sử lấy từ dữ liệu hoặc tính toán)
        order_count = int(itemset.support * 100)  # Ví dụ tính toán theo tỷ lệ

        groups.append({
            'group_name': group_name,
            'group_items': group_items,
            'order_count': order_count
        })
        group_counter += 1

    return {
        'total_groups': len(groups),
        'groups': groups
    }


from models.pydantic_models import AnalysisResult, FrequentlyBoughtTogether

def perform_frequent_product_analysis(
    products: List[ProductInputModel],
    order_details: List[OrderDetailInputModel],
    min_support: float,
    min_confidence: float
) -> FrequentlyBoughtTogether:
    """
    Performs the full frequent product analysis.

    Args:
        products: List of product details.
        order_details: List of order details.
        min_support: Minimum support threshold.
        min_confidence: Minimum confidence threshold.

    Returns:
        An instance of FrequentlyBoughtTogether containing frequent itemsets and product groups.
    """
    if not products or not order_details:
        raise HTTPException(status_code=400, detail="Product list and order details cannot be empty.")

    product_map = _create_product_map(products)
    if not product_map:
        raise HTTPException(status_code=400, detail="Product list is empty or yields no usable mapping.")

    transactions = _prepare_transactions(order_details)
    if not transactions:
        raise HTTPException(status_code=400, detail="No transactions could be formed from the order details.")

    frequent_itemsets_df = _run_fpgrowth_analysis(transactions, min_support)
    if frequent_itemsets_df is None:
        raise HTTPException(status_code=400, detail=f"No frequent itemsets found with min_support {min_support}.")

    rules_df = _generate_association_rules(frequent_itemsets_df, min_confidence)

    output_itemsets, output_rules = _format_results(frequent_itemsets_df, rules_df, product_map)

    # Kiểm tra nếu không có itemsets hoặc rules
    if not output_itemsets:
        raise HTTPException(status_code=400, detail="No frequent itemsets found.")
    if not output_rules:
        raise HTTPException(status_code=400, detail="No association rules found.")

    print("Product to transform:", products)
    print("Frequent itemsets to transform:", frequent_itemsets_df)
    # Sử dụng transform_data để chuẩn bị kết quả trả về
    transformed_data = transform_data(output_itemsets, products)

    return FrequentlyBoughtTogether(
        total_groups=transformed_data['total_groups'],
        groups=transformed_data['groups'],
        # frequent_itemsets=output_itemsets,  # Đảm bảo trả về frequent_itemsets
        # association_rules=output_rules     # Đảm bảo trả về association_rules
    )
