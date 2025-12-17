from __future__ import annotations
from typing import List
from sqlalchemy.orm import Mapped, mapped_column, relationship
from sqlalchemy import Integer, String
from .student import Base

class Subject(Base):
    __tablename__ = "subjects"

    id: Mapped[int] = mapped_column(Integer, primary_key=True)
    name: Mapped[str] = mapped_column(String(200), nullable=False, unique=True)
    credits: Mapped[int] = mapped_column(Integer, nullable=False, default=3)
    semester: Mapped[int] = mapped_column(Integer, nullable=False, default=1)

    grades: Mapped[List["Grade"]] = relationship(back_populates="subject")
