from __future__ import annotations
from fastapi import APIRouter

from .students import router as students_router
from .subjects import router as subjects_router
from .grades import router as grades_router

router = APIRouter()
router.include_router(students_router)
router.include_router(subjects_router)
router.include_router(grades_router)
