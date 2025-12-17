from __future__ import annotations
from pydantic import BaseModel


class SubjectCreate(BaseModel):
    name: str
    credits: int
    semester: int


class SubjectRead(BaseModel):
    id: int
    name: str
    credits: int
    semester: int
