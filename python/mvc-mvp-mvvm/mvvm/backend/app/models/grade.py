from __future__ import annotations
from datetime import date
from typing import ClassVar, Iterable, Tuple, TypeVar
from sqlalchemy.orm import Mapped, mapped_column, relationship
from sqlalchemy import Integer, String, Date, ForeignKey, CheckConstraint, UniqueConstraint
from .student import Base

T = TypeVar("T")

class Grade(Base):
    __tablename__ = "grades"

    id: Mapped[int] = mapped_column(Integer, primary_key=True)
    student_id: Mapped[int] = mapped_column(Integer, ForeignKey("students.id", ondelete="CASCADE"), nullable=False)
    subject_id: Mapped[int] = mapped_column(Integer, ForeignKey("subjects.id", ondelete="CASCADE"), nullable=False)

    value: Mapped[int] = mapped_column(Integer, nullable=False)  # 0..100
    grade_type: Mapped[str] = mapped_column(String(30), nullable=False)
    grade_date: Mapped[date] = mapped_column(Date, nullable=False)

    student: Mapped["Student"] = relationship(back_populates="grades")
    subject: Mapped["Subject"] = relationship(back_populates="grades")

    __table_args__ = (
        CheckConstraint("value >= 0 AND value <= 100", name="ck_grade_value_0_100"),
        UniqueConstraint("student_id", "subject_id", "grade_type", "grade_date", name="uq_grade"),
    )

    ECTS_BOUNDS: ClassVar[list[tuple[int, str]]] = [(90,"A"),(82,"B"),(74,"C"),(64,"D"),(60,"E"),(35,"FX"),(0,"F")]
    FIVE_BOUNDS: ClassVar[list[tuple[int, int]]] = [(90,5),(74,4),(60,3),(0,2)]

    @staticmethod
    def _map_by_bounds(value: int, bounds: Iterable[Tuple[int, T]]) -> T:
        v = int(value)
        last: T | None = None
        for min_v, out in bounds:
            last = out
            if v >= min_v:
                return out
        assert last is not None
        return last

    def ects_letter(self) -> str:
        return self._map_by_bounds(self.value, self.ECTS_BOUNDS)

    def five_point(self) -> int:
        return self._map_by_bounds(self.value, self.FIVE_BOUNDS)
