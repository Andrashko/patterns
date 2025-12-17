from __future__ import annotations
from sqlalchemy.orm import Session
from sqlalchemy import select

from ..models.student import Student
from ..schemas.student import StudentCreate, StudentRead


def list_students(db: Session) -> list[StudentRead]:
    items = db.execute(select(Student).order_by(Student.full_name.asc())).scalars().all()
    return [StudentRead(id=s.id, full_name=s.full_name, group=s.group, enrollment_year=s.enrollment_year) for s in items]


def create_student(db: Session, payload: StudentCreate) -> StudentRead:
    s = Student(**payload.model_dump())
    db.add(s)
    db.commit()
    db.refresh(s)
    return StudentRead(id=s.id, full_name=s.full_name, group=s.group, enrollment_year=s.enrollment_year)


def delete_student(db: Session, student_id: int) -> None:
    s = db.get(Student, student_id)
    if s is None:
        return
    db.delete(s)
    db.commit()
