from __future__ import annotations

# Імпорти моделей важливі: щоб SQLAlchemy знав мапінги для relationship
from app.models.student import Student  # noqa: F401
from app.models.subject import Subject  # noqa: F401
from app.models.grade import Grade      # noqa: F401

from app.views.main_view import MainView
from app.presenters.students_presenter import StudentsPresenter
from app.presenters.subjects_presenter import SubjectsPresenter
from app.presenters.grades_presenter import GradesPresenter

def main() -> None:
    view = MainView()

    students_p = StudentsPresenter(view=view)
    subjects_p = SubjectsPresenter(view=view)
    grades_p = GradesPresenter(view=view)

    view.set_presenters(students_p, subjects_p, grades_p)

    # Завантаження даних у всі вкладки
    students_p.load()
    subjects_p.load()
    grades_p.load()

    view.mainloop()

if __name__ == "__main__":
    main()
