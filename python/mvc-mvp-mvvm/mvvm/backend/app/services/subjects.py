from __future__ import annotations
from sqlalchemy.orm import Session
from sqlalchemy import select

from ..models.subject import Subject
from ..schemas.subject import SubjectCreate, SubjectRead


def list_subjects(db: Session) -> list[SubjectRead]:
    items = db.execute(select(Subject).order_by(Subject.name.asc())).scalars().all()
    return [SubjectRead(id=s.id, name=s.name, credits=s.credits, semester=s.semester) for s in items]


def create_subject(db: Session, payload: SubjectCreate) -> SubjectRead:
    sub = Subject(**payload.model_dump())
    db.add(sub)
    db.commit()
    db.refresh(sub)
    return SubjectRead(id=sub.id, name=sub.name, credits=sub.credits, semester=sub.semester)


def delete_subject(db: Session, subject_id: int) -> None:
    sub = db.get(Subject, subject_id)
    if sub is None:
        return
    db.delete(sub)
    db.commit()
