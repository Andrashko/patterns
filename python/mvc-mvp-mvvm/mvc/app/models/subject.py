from __future__ import annotations
from typing import List
from ..extensions import db

class Subject(db.Model):
    __tablename__ = "subjects"

    id: db.Mapped[int] = db.mapped_column(db.Integer, primary_key=True)
    name: db.Mapped[str] = db.mapped_column(db.String(200), nullable=False, unique=True)
    credits: db.Mapped[int] = db.mapped_column(db.Integer, nullable=False, default=3)
    semester: db.Mapped[int] = db.mapped_column(db.Integer, nullable=False, default=1)

    grades: db.Mapped[List["Grade"]] = db.relationship(
        "Grade", back_populates="subject", cascade="all, delete-orphan"
    )

    def __repr__(self) -> str:
        return f"<Subject {self.id} {self.name}>"
