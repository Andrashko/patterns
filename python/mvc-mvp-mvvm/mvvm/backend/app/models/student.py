from __future__ import annotations
from typing import List
from sqlalchemy.orm import DeclarativeBase, Mapped, mapped_column, relationship
from sqlalchemy import Integer, String

class Base(DeclarativeBase):
    pass

class Student(Base):
    __tablename__ = "students"

    id: Mapped[int] = mapped_column(Integer, primary_key=True)
    full_name: Mapped[str] = mapped_column(String(200), nullable=False)
    group: Mapped[str] = mapped_column(String(50), nullable=False)
    enrollment_year: Mapped[int] = mapped_column(Integer, nullable=False)

    grades: Mapped[List["Grade"]] = relationship(back_populates="student")
