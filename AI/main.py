from fastapi import FastAPI
from controllers.analysis_controller import router as analysis_router

app = FastAPI(
    title="Product Association Analyzer",
    description="API to find frequently co-purchased products using FP-Growth",
    version="1.1.0",
)

app.include_router(analysis_router, tags=["Analysis"])

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="localhost", port=5000)
