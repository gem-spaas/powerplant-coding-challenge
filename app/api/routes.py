from fastapi import APIRouter

router = APIRouter()


@router.get("/healthcheck")
async def healthcheck():
    return {"status": "ok"}


@router.get("/productionplan")
async def production_plan():
    # TODO
    return {"message": "not implemented"}
