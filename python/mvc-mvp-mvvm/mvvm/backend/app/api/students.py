from __future__ import annotations
from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session

from ..core.db import get_session
from ..schemas.student import StudentCreate, StudentRead
from ..services.students import list_students, create_student, delete_student

router = APIRouter(prefix="/students", tags=["students"])


@router.get("", response_model=list[StudentRead])
def get_students(db: Session = Depends(get_session)):
    return list_students(db)


@router.post("", response_model=StudentRead)
def post_student(payload: StudentCreate, db: Session = Depends(get_session)):
    return create_student(db, payload)


@router.delete("/{student_id}")
def delete_student_ep(student_id: int, db: Session = Depends(get_session)):
    delete_student(db, student_id)
    return {"ok": True}
