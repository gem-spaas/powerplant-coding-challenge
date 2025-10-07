from fastapi.exceptions import RequestValidationError
from starlette.responses import JSONResponse
from fastapi import Request


async def validation_exception_handler(request: Request, exc: RequestValidationError):
    return JSONResponse(
        status_code=400,
        content={
            "success": False,
            "message": "Validation failed",
            "errors": exc.errors(),
        },
    )

# Handle all unhandled server errors
async def server_error_handler(request: Request, exc: Exception):
    # Log the exception if needed here
    return JSONResponse(
        status_code=500,
        content={
            "success": False,
            "message": "An unexpected error occurred. Please try again later.",
        },
    )