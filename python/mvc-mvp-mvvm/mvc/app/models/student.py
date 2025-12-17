from __future__ import annotations
from typing import List
from ..extensions import db

class Student(db.Model):
    __tablename__ = "students"

    id: db.Mapped[int] = db.mapped_column(db.Integer, primary_key=True)
    full_name: db.Mapped[str] = db.mapped_column(db.String(200), nullable=False)
    group: db.Mapped[str] = db.mapped_column(db.String(50), nullable=False)
    enrollment_year: db.Mapped[int] = db.mapped_column(db.Integer, nullable=False)

    grades: db.Mapped[List["Grade"]] = db.relationship(
        "Grade", back_populates="student", cascade="all, delete-orphan"
    )

    def __repr__(self) -> str:
        return f"<Student {self.id} {self.full_name}>"
