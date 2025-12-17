from __future__ import annotations
from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session

from ..core.db import get_session
from ..schemas.grade import GradeCreate, GradeRead
from ..services.grades import list_grades, create_grade, delete_grade

router = APIRouter(prefix="/grades", tags=["grades"])


@router.get("", response_model=list[GradeRead])
def get_grades(db: Session = Depends(get_session)):
    return list_grades(db)


@router.post("", response_model=GradeRead)
def post_grade(payload: GradeCreate, db: Session = Depends(get_session)):
    return create_grade(db, payload)


@router.delete("/{grade_id}")
def delete_grade_ep(grade_id: int, db: Session = Depends(get_session)):
    delete_grade(db, grade_id)
    return {"ok": True}
