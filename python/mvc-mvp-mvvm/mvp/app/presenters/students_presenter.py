from __future__ import annotations
from ..db import SessionFactory
from ..data.repositories import StudentRepo
from ..models.student import Student
from ..views.interfaces import StudentsView

class StudentsPresenter:
    def __init__(self, view: StudentsView):
        self.view = view

    def load(self) -> None:
        with SessionFactory() as session:
            repo = StudentRepo(session)
            rows = [
                {"id": s.id, "full_name": s.full_name, "group": s.group, "enrollment_year": s.enrollment_year}
                for s in repo.list()
            ]
        self.view.show_students(rows)

    def create(self) -> None:
        data = self.view.get_student_form_data()
        full_name = (data.get("full_name") or "").strip()
        group = (data.get("group") or "").strip()
        year_s = str(data.get("enrollment_year") or "").strip()

        if not full_name or not group or not year_s.isdigit():
            self.view.show_message("Некоректні дані студента.", "error")
            return

        with SessionFactory() as session:
            repo = StudentRepo(session)
            repo.add(Student(full_name=full_name, group=group, enrollment_year=int(year_s)))
            session.commit()

        self.view.show_message("Студента додано.", "success")
        self.view.clear_student_form()
        self.load()

    def delete_selected(self) -> None:
        student_id = self.view.get_selected_student_id()
        if student_id is None:
            self.view.show_message("Оберіть студента.", "error")
            return
        if not self.view.confirm("Вилучити обраного студента?"):
            return

        with SessionFactory() as session:
            repo = StudentRepo(session)
            s = repo.get(student_id)
            if s is None:
                self.view.show_message("Студента не знайдено.", "error")
                return
            repo.delete(s)
            session.commit()

        self.view.show_message("Студента вилучено.", "success")
        self.load()
