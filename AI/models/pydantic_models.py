from typing import Optional, List, Tuple
from pydantic import BaseModel, Field

# Input
class ProductInputModel(BaseModel):
    product_id: int = Field(..., alias="ProductId")
    product_code: str = Field(..., alias="ProductCode")
    product_name: str = Field(..., alias="ProductName")
    product_category: Optional[str] = Field(None, alias="ProductCategory")

    class Config:
        validate_by_name = True

# Input
class OrderDetailInputModel(BaseModel):
    order_id: int = Field(..., alias='OrderId')
    product_id: int = Field(..., alias='ProductId')
    quantity: int = Field(..., alias='Quantity', ge=1)

    class Config:
        validate_by_name = True

# Dữ liệu đầu vào tổng
class AnalysisInput(BaseModel):
    products: List[ProductInputModel]
    order_details: List[OrderDetailInputModel]

# Xác định độ phổ biến của itemset
class FrequentItemset(BaseModel):
    itemset: Tuple[str, ...]
    support: float

# Quy tắc kết hợp
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
