from __future__ import annotations
from alembic import op
import sqlalchemy as sa
from datetime import date

revision = "0002"
down_revision = "0001"
branch_labels = None
depends_on = None

def upgrade() -> None:
    students = sa.table(
        "students",
        sa.column("id", sa.Integer),
        sa.column("full_name", sa.String),
        sa.column("group", sa.String),
        sa.column("enrollment_year", sa.Integer),
    )
    subjects = sa.table(
        "subjects",
        sa.column("id", sa.Integer),
        sa.column("name", sa.String),
        sa.column("credits", sa.Integer),
        sa.column("semester", sa.Integer),
    )
    grades = sa.table(
        "grades",
        sa.column("id", sa.Integer),
        sa.column("student_id", sa.Integer),
        sa.column("subject_id", sa.Integer),
        sa.column("value", sa.Integer),
        sa.column("grade_type", sa.String),
        sa.column("grade_date", sa.Date),
    )

    # 3 students
    op.bulk_insert(students, [
        {"id": 1, "full_name": "Іваненко Іван Іванович", "group": "SA-21", "enrollment_year": 2023},
        {"id": 2, "full_name": "Петренко Марія Олегівна", "group": "SA-21", "enrollment_year": 2023},
        {"id": 3, "full_name": "Шевченко Андрій Сергійович", "group": "DS-11", "enrollment_year": 2024},
    ])

    # 3 subjects
    op.bulk_insert(subjects, [
        {"id": 1, "name": "Системний аналіз", "credits": 4, "semester": 3},
        {"id": 2, "name": "Бази даних", "credits": 4, "semester": 2},
        {"id": 3, "name": "Програмування Python", "credits": 5, "semester": 1},
    ])

    # 4 grades (разом 10 рядків у БД після міграції)
    op.bulk_insert(grades, [
        {"id": 1, "student_id": 1, "subject_id": 1, "value": 88, "grade_type": "exam",   "grade_date": date(2025, 12, 1)},
        {"id": 2, "student_id": 1, "subject_id": 2, "value": 74, "grade_type": "final",  "grade_date": date(2025, 12, 2)},
        {"id": 3, "student_id": 2, "subject_id": 1, "value": 95, "grade_type": "exam",   "grade_date": date(2025, 12, 1)},
        {"id": 4, "student_id": 3, "subject_id": 3, "value": 61, "grade_type": "module", "grade_date": date(2025, 11, 20)},
    ])

def downgrade() -> None:
    # простий rollback (очистити таблиці)
    op.execute("DELETE FROM grades")
    op.execute("DELETE FROM subjects")
    op.execute("DELETE FROM students")
