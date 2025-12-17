from __future__ import annotations
from sqlalchemy.orm import Session
from sqlalchemy import select

from ..models.grade import Grade
from ..schemas.grade import GradeCreate, GradeRead


def list_grades(db: Session) -> list[GradeRead]:
    items = db.execute(select(Grade).order_by(Grade.grade_date.desc(), Grade.id.desc())).scalars().all()
    return [
        GradeRead(
            id=g.id,
            student_id=g.student_id,
            subject_id=g.subject_id,
            student_name=g.student.full_name,
            subject_name=g.subject.name,
            value=g.value,
            ects=g.ects_letter(),
            five=g.five_point(),
            grade_type=g.grade_type,
            grade_date=g.grade_date,
        )
        for g in items
    ]


def create_grade(db: Session, payload: GradeCreate) -> GradeRead:
    g = Grade(**payload.model_dump())
    db.add(g)
    db.commit()
    db.refresh(g)
    return GradeRead(
        id=g.id,
        student_id=g.student_id,
        subject_id=g.subject_id,
        student_name=g.student.full_name,
        subject_name=g.subject.name,
        value=g.value,
        ects=g.ects_letter(),
        five=g.five_point(),
        grade_type=g.grade_type,
        grade_date=g.grade_date,
    )


def delete_grade(db: Session, grade_id: int) -> None:
    g = db.get(Grade, grade_id)
    if g is None:
        return
    db.delete(g)
    db.commit()
