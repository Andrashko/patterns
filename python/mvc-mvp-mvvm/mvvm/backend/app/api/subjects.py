from __future__ import annotations
from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session

from ..core.db import get_session
from ..schemas.subject import SubjectCreate, SubjectRead
from ..services.subjects import list_subjects, create_subject, delete_subject

router = APIRouter(prefix="/subjects", tags=["subjects"])


@router.get("", response_model=list[SubjectRead])
def get_subjects(db: Session = Depends(get_session)):
    return list_subjects(db)


@router.post("", response_model=SubjectRead)
def post_subject(payload: SubjectCreate, db: Session = Depends(get_session)):
    return create_subject(db, payload)


@router.delete("/{subject_id}")
def delete_subject_ep(subject_id: int, db: Session = Depends(get_session)):
    delete_subject(db, subject_id)
    return {"ok": True}
