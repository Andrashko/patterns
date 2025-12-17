from __future__ import annotations
from ..db import SessionFactory
from ..data.repositories import SubjectRepo
from ..models.subject import Subject
from ..views.interfaces import SubjectsView

class SubjectsPresenter:
    def __init__(self, view: SubjectsView):
        self.view = view

    def load(self) -> None:
        with SessionFactory() as session:
            repo = SubjectRepo(session)
            rows = [{"id": s.id, "name": s.name, "credits": s.credits, "semester": s.semester} for s in repo.list()]
        self.view.show_subjects(rows)

    def create(self) -> None:
        data = self.view.get_subject_form_data()
        name = (data.get("name") or "").strip()
        credits_s = str(data.get("credits") or "").strip()
        semester_s = str(data.get("semester") or "").strip()

        if not name or not credits_s.isdigit() or not semester_s.isdigit():
            self.view.show_message("Некоректні дані предмета.", "error")
            return

        with SessionFactory() as session:
            repo = SubjectRepo(session)
            repo.add(Subject(name=name, credits=int(credits_s), semester=int(semester_s)))
            session.commit()

        self.view.show_message("Предмет додано.", "success")
        self.view.clear_subject_form()
        self.load()

    def delete_selected(self) -> None:
        subject_id = self.view.get_selected_subject_id()
        if subject_id is None:
            self.view.show_message("Оберіть предмет.", "error")
            return
        if not self.view.confirm("Вилучити обраний предмет?"):
            return

        with SessionFactory() as session:
            repo = SubjectRepo(session)
            s = repo.get(subject_id)
            if s is None:
                self.view.show_message("Предмет не знайдено.", "error")
                return
            repo.delete(s)
            session.commit()

        self.view.show_message("Предмет вилучено.", "success")
        self.load()
