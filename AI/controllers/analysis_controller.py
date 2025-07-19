from fastapi import APIRouter, HTTPException, Query, Depends
from models.pydantic_models import AnalysisInput, AnalysisResult
from services.analysis_service import perform_frequent_product_analysis

router = APIRouter()

@router.post(
    "/analyze/frequent-products",
    response_model=AnalysisResult,
    response_model_exclude_none=True,
    summary="Analyze Frequent Product Associations",
    description="Find frequently co-purchased products using FP-Growth and generate association rules."
)
async def analyze_frequent_products_endpoint(
    input_data: AnalysisInput,
    min_support: float = Query(0.1, ge=0.01, le=1.0, description="Minimum support threshold (proportion: 0.01 to 1.0)"),
    min_confidence: float = Query(0.5, ge=0.0, le=1.0, description="Minimum confidence threshold (0.0 to 1.0)")
):
    """
    Analyzes product purchase patterns to find frequent itemsets and association rules.

    - **input_data**: Contains lists of products and order details.
    - **min_support**: The minimum frequency (as a proportion) for an itemset to be considered frequent.
    - **min_confidence**: The minimum confidence for an association rule to be generated.
    """
    if not input_data.products or not input_data.order_details:
        raise HTTPException(status_code=400, detail="Product list and order details cannot be empty.")

    try:
        itemsets, rules, message = perform_frequent_product_analysis(
            products=input_data.products,
            order_details=input_data.order_details,
            min_support=min_support,
            min_confidence=min_confidence
        )

        return AnalysisResult(
            frequent_itemsets=itemsets,
            association_rules=rules,
            message=message
        )

    except Exception as e:
        print(f"An unexpected error occurred: {e}")
        raise HTTPException(status_code=500, detail="An internal server error occurred during analysis.")

@router.get(
    "/",
    summary="API Root",
    description="Welcome message for the Product Association Analyzer API."
    )
async def read_root_endpoint():
    """Returns a simple welcome message."""
    return {"message": "Hello World!"}