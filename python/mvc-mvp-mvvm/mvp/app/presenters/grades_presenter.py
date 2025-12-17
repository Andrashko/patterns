from __future__ import annotations
from datetime import date
from ..db import SessionFactory
from ..data.repositories import GradeRepo
from ..models.grade import Grade
from ..views.interfaces import GradesView

ALLOWED_TYPES: set[str] = {"current", "module", "exam", "final"}

class GradesPresenter:
    def __init__(self, view: GradesView):
        self.view = view

    def load(self) -> None:
        with SessionFactory() as session:
            repo = GradeRepo(session)
            grades = repo.list()
            rows = [
                {
                    "id": g.id,
                    "date": g.grade_date.isoformat(),
                    "student": g.student.full_name,
                    "subject": g.subject.name,
                    "type": g.grade_type,
                    "value": g.value,
                    "ects": g.ects_letter(),
                    "five": g.five_point(),
                }
                for g in grades
            ]
        self.view.show_grades(rows)

    def create(self) -> None:
        data = self.view.get_grade_form_data()
        student_id_s = str(data.get("student_id") or "").strip()
        subject_id_s = str(data.get("subject_id") or "").strip()
        value_s = str(data.get("value") or "").strip()
        grade_type = (data.get("grade_type") or "").strip()
        date_s = (data.get("grade_date") or "").strip()

        if not (student_id_s.isdigit() and subject_id_s.isdigit() and value_s.isdigit()):
            self.view.show_message("student_id, subject_id, value мають бути числами.", "error")
            return

        v = int(value_s)
        if v < 0 or v > 100:
            self.view.show_message("Оцінка має бути 0..100.", "error")
            return

        if grade_type not in ALLOWED_TYPES:
            self.view.show_message("Некоректний тип оцінки.", "error")
            return

        try:
            d = date.fromisoformat(date_s) if date_s else date.today()
        except ValueError:
            self.view.show_message("Дата має бути YYYY-MM-DD.", "error")
            return

        with SessionFactory() as session:
            repo = GradeRepo(session)
            repo.add(Grade(
                student_id=int(student_id_s),
                subject_id=int(subject_id_s),
                value=v,
                grade_type=grade_type,
                grade_date=d,
            ))
            session.commit()

        self.view.show_message("Оцінку додано.", "success")
        self.view.clear_grade_form()
        self.load()

    def delete_selected(self) -> None:
        grade_id = self.view.get_selected_grade_id()
        if grade_id is None:
            self.view.show_message("Оберіть оцінку.", "error")
            return
        if not self.view.confirm("Вилучити обрану оцінку?"):
            return

        with SessionFactory() as session:
            repo = GradeRepo(session)
            g = repo.get(grade_id)
            if g is None:
                self.view.show_message("Оцінку не знайдено.", "error")
                return
            repo.delete(g)
            session.commit()

        self.view.show_message("Оцінку вилучено.", "success")
        self.load()
