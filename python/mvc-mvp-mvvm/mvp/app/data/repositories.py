from __future__ import annotations
from datetime import date
from sqlalchemy import select
from sqlalchemy.orm import Session
from ..models.student import Student
from ..models.subject import Subject
from ..models.grade import Grade

class StudentRepo:
    def __init__(self, session: Session):
        self.session = session

    def list(self) -> list[Student]:
        return self.session.execute(select(Student).order_by(Student.full_name.asc())).scalars().all()

    def add(self, s: Student) -> None:
        self.session.add(s)

    def get(self, student_id: int) -> Student | None:
        return self.session.get(Student, student_id)

    def delete(self, s: Student) -> None:
        self.session.delete(s)

class SubjectRepo:
    def __init__(self, session: Session):
        self.session = session

    def list(self) -> list[Subject]:
        return self.session.execute(select(Subject).order_by(Subject.name.asc())).scalars().all()

    def add(self, sub: Subject) -> None:
        self.session.add(sub)

    def get(self, subject_id: int) -> Subject | None:
        return self.session.get(Subject, subject_id)

    def delete(self, sub: Subject) -> None:
        self.session.delete(sub)

class GradeRepo:
    def __init__(self, session: Session):
        self.session = session

    def list(self) -> list[Grade]:
        stmt = select(Grade).order_by(Grade.grade_date.desc(), Grade.id.desc())
        return self.session.execute(stmt).scalars().all()

    def add(self, g: Grade) -> None:
        self.session.add(g)

    def get(self, grade_id: int) -> Grade | None:
        return self.session.get(Grade, grade_id)

    def delete(self, g: Grade) -> None:
        self.session.delete(g)
