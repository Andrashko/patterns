from __future__ import annotations
from datetime import date
from typing import Any, ClassVar, Iterable, Tuple, TypeVar
from ..extensions import db

T = TypeVar("T")

class Grade(db.Model):
    __tablename__ = "grades"

    id: db.Mapped[int] = db.mapped_column(db.Integer, primary_key=True)

    student_id: db.Mapped[int] = db.mapped_column(
        db.Integer, db.ForeignKey("students.id", ondelete="CASCADE"), nullable=False
    )
    subject_id: db.Mapped[int] = db.mapped_column(
        db.Integer, db.ForeignKey("subjects.id", ondelete="CASCADE"), nullable=False
    )

    value: db.Mapped[int] = db.mapped_column(db.Integer, nullable=False)  # 0..100
    grade_type: db.Mapped[str] = db.mapped_column(db.String(30), nullable=False)
    grade_date: db.Mapped[date] = db.mapped_column(db.Date, nullable=False, default=date.today)

    student: db.Mapped["Student"] = db.relationship("Student", back_populates="grades")
    subject: db.Mapped["Subject"] = db.relationship("Subject", back_populates="grades")

    __table_args__ = (
        db.CheckConstraint("value >= 0 AND value <= 100", name="ck_grade_value_0_100"),
        db.UniqueConstraint("student_id", "subject_id", "grade_type", "grade_date", name="uq_grade"),
    )

    # ==== bounds from you ====
    ECTS_BOUNDS: ClassVar[list[tuple[int, str]]] = [(90,"A"),(82,"B"),(74,"C"),(64,"D"),(60,"E"),(35,"FX"),(0,"F")]
    FIVE_BOUNDS: ClassVar[list[tuple[int, int]]] = [(90,5),(74,4),(60,3),(0,2)]

    @staticmethod
    def _map_by_bounds(value: int, bounds: Iterable[Tuple[int, T]]) -> T:
        v = int(value)
        last: Any = None
        for min_v, out in bounds:
            last = out
            if v >= min_v:
                return out
        return last  # bounds містять 0, тож сюди фактично не дійдемо

    def ects_letter(self) -> str:
        return self._map_by_bounds(self.value, self.ECTS_BOUNDS)

    def five_point(self) -> int:
        return self._map_by_bounds(self.value, self.FIVE_BOUNDS)

    def presentation(self) -> str:
        return f"{self.value} / ECTS {self.ects_letter()} / {self.five_point()}-бальна"

    def __repr__(self) -> str:
        return f"<Grade s={self.student_id} sub={self.subject_id} v={self.value}>"
