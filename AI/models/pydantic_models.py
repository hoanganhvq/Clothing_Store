from typing import Optional, List, Tuple
from pydantic import BaseModel, Field


class Category(BaseModel):
    name: str

# Input
class ProductInputModel(BaseModel):
    product_id: int = Field(..., alias="id")
    product_code: str = Field(..., alias="productCode")
    product_name: str = Field(..., alias="name")
    product_category: Optional[str] = Field(None, alias="categoryName")
    product_price: float = Field(..., alias="price")
    product_cost_price: float = Field(..., alias="costPrice")
    product_stock_quantity: int = Field(..., alias="stockQuantity")
    product_discount: float = Field(..., alias="discount")
    product_image_path: str = Field(..., alias="imageUrl")
    product_category_id: int = Field(..., alias="categoryId")
    class Config:
        validate_by_name = True

# Input
class OrderDetailInputModel(BaseModel):
    order_id: int = Field(..., alias='orderId')
    product_id: int = Field(..., alias='productId')
    quantity: int = Field(..., alias='quantity', ge=1)

    class Config:
        validate_by_name = True

# Dữ liệu đầu vào tổng
class AnalysisInput(BaseModel):
    products: List[ProductInputModel]
    order_details: List[OrderDetailInputModel]

# Xác định độ phổ biến của itemset
class FrequentItemset(BaseModel):
    itemset: Tuple[int, ...]
    support: float

class AssociationRule(BaseModel):
    antecedents: Tuple[int, ...]
    consequents: Tuple[int, ...]
    antecedent_support: float
    consequent_support: float
    support: float
    confidence: float
    lift: Optional[float]
    leverage: float
    conviction: Optional[float]

class AnalysisResult(BaseModel):
    frequent_itemsets: List[FrequentItemset]
    association_rules: List[AssociationRule]
    message: Optional[str] = None

class Product(BaseModel):
    id: int
    productCode: str
    name: str
    price: float
    stockQuantity: int
    costPrice: float
    discount: float
    imageUrl: str
    categoryName: str
    categoryId: int
class ProductGroup(BaseModel):
    group_name: str
    group_items: List[Product]
    order_count: int

class FrequentlyBoughtTogether(BaseModel):
    total_groups: int
    groups: List[ProductGroup]
    # frequent_itemsets: List[FrequentItemset]  # Thêm trường frequent_itemsets
    # association_rules: List[AssociationRule]  # Thêm trường association_rules
