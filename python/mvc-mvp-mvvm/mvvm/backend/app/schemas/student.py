from __future__ import annotations
from pydantic import BaseModel


class StudentCreate(BaseModel):
    full_name: str
    group: str
    enrollment_year: int


class StudentRead(BaseModel):
    id: int
    full_name: str
    group: str
    enrollment_year: int
