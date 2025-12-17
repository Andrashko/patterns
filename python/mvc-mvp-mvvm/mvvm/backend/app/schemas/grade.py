from __future__ import annotations
from datetime import date
from pydantic import BaseModel, Field


class GradeCreate(BaseModel):
    student_id: int
    subject_id: int
    value: int = Field(ge=0, le=100)
    grade_type: str
    grade_date: date


class GradeRead(BaseModel):
    id: int
    student_id: int
    subject_id: int
    student_name: str
    subject_name: str
    value: int
    ects: str
    five: int
    grade_type: str
    grade_date: date
