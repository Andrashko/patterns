from __future__ import annotations
import tkinter as tk
from tkinter import ttk, messagebox
from typing import Sequence

from .interfaces import StudentsView, SubjectsView, GradesView

class MainView(tk.Tk, StudentsView, SubjectsView, GradesView):
    def __init__(self) -> None:
        super().__init__()
        self.title("Student Grades — Desktop MVP")
        self.geometry("1100x520")

        self.students_presenter = None
        self.subjects_presenter = None
        self.grades_presenter = None

        nb = ttk.Notebook(self)
        nb.pack(fill=tk.BOTH, expand=True)

        self.tab_students = ttk.Frame(nb, padding=10)
        self.tab_subjects = ttk.Frame(nb, padding=10)
        self.tab_grades = ttk.Frame(nb, padding=10)

        nb.add(self.tab_students, text="Студенти")
        nb.add(self.tab_subjects, text="Предмети")
        nb.add(self.tab_grades, text="Оцінки")

        self._build_students_tab()
        self._build_subjects_tab()
        self._build_grades_tab()

    def set_presenters(self, students_p, subjects_p, grades_p) -> None:
        self.students_presenter = students_p
        self.subjects_presenter = subjects_p
        self.grades_presenter = grades_p

    # ---------- Students tab ----------
    def _build_students_tab(self) -> None:
        left = ttk.Frame(self.tab_students)
        left.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)

        self.students_tree = ttk.Treeview(
            left, columns=("id", "full_name", "group", "year"), show="headings", height=16
        )
        for col, title, w in [
            ("id","ID",60), ("full_name","ПІБ",360), ("group","Група",120), ("year","Рік",120)
        ]:
            self.students_tree.heading(col, text=title)
            self.students_tree.column(col, width=w, anchor=tk.CENTER if col in ("id","group","year") else tk.W)
        self.students_tree.pack(fill=tk.BOTH, expand=True)

        right = ttk.Frame(self.tab_students, padding=(10,0,0,0))
        right.pack(side=tk.RIGHT, fill=tk.Y)

        self.st_full_name = tk.StringVar()
        self.st_group = tk.StringVar()
        self.st_year = tk.StringVar()

        ttk.Label(right, text="Додати студента", font=("Segoe UI", 11, "bold")).pack(anchor="w", pady=(0,8))
        ttk.Label(right, text="ПІБ").pack(anchor="w")
        ttk.Entry(right, textvariable=self.st_full_name, width=32).pack(anchor="w", pady=(0,8))
        ttk.Label(right, text="Група").pack(anchor="w")
        ttk.Entry(right, textvariable=self.st_group, width=32).pack(anchor="w", pady=(0,8))
        ttk.Label(right, text="Рік вступу").pack(anchor="w")
        ttk.Entry(right, textvariable=self.st_year, width=32).pack(anchor="w", pady=(0,12))

        ttk.Button(right, text="Додати", command=lambda: self.students_presenter.create()).pack(fill=tk.X, pady=(0,6))
        ttk.Button(right, text="Вилучити обраного", command=lambda: self.students_presenter.delete_selected()).pack(fill=tk.X)

    def show_students(self, rows: Sequence[dict]) -> None:
        for i in self.students_tree.get_children():
            self.students_tree.delete(i)
        for r in rows:
            self.students_tree.insert("", "end", values=(r["id"], r["full_name"], r["group"], r["enrollment_year"]))

    def get_student_form_data(self) -> dict:
        return {"full_name": self.st_full_name.get(), "group": self.st_group.get(), "enrollment_year": self.st_year.get()}

    def clear_student_form(self) -> None:
        self.st_full_name.set(""); self.st_group.set(""); self.st_year.set("")

    def get_selected_student_id(self) -> int | None:
        sel = self.students_tree.selection()
        if not sel:
            return None
        return int(self.students_tree.item(sel[0], "values")[0])

    # ---------- Subjects tab ----------
    def _build_subjects_tab(self) -> None:
        left = ttk.Frame(self.tab_subjects)
        left.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)

        self.subjects_tree = ttk.Treeview(
            left, columns=("id","name","credits","semester"), show="headings", height=16
        )
        for col, title, w in [
            ("id","ID",60), ("name","Назва",420), ("credits","Кредити",100), ("semester","Семестр",100)
        ]:
            self.subjects_tree.heading(col, text=title)
            self.subjects_tree.column(col, width=w, anchor=tk.CENTER if col in ("id","credits","semester") else tk.W)
        self.subjects_tree.pack(fill=tk.BOTH, expand=True)

        right = ttk.Frame(self.tab_subjects, padding=(10,0,0,0))
        right.pack(side=tk.RIGHT, fill=tk.Y)

        self.sub_name = tk.StringVar()
        self.sub_credits = tk.StringVar(value="3")
        self.sub_semester = tk.StringVar(value="1")

        ttk.Label(right, text="Додати предмет", font=("Segoe UI", 11, "bold")).pack(anchor="w", pady=(0,8))
        ttk.Label(right, text="Назва").pack(anchor="w")
        ttk.Entry(right, textvariable=self.sub_name, width=32).pack(anchor="w", pady=(0,8))
        ttk.Label(right, text="Кредити").pack(anchor="w")
        ttk.Entry(right, textvariable=self.sub_credits, width=32).pack(anchor="w", pady=(0,8))
        ttk.Label(right, text="Семестр").pack(anchor="w")
        ttk.Entry(right, textvariable=self.sub_semester, width=32).pack(anchor="w", pady=(0,12))

        ttk.Button(right, text="Додати", command=lambda: self.subjects_presenter.create()).pack(fill=tk.X, pady=(0,6))
        ttk.Button(right, text="Вилучити обраний", command=lambda: self.subjects_presenter.delete_selected()).pack(fill=tk.X)

    def show_subjects(self, rows: Sequence[dict]) -> None:
        for i in self.subjects_tree.get_children():
            self.subjects_tree.delete(i)
        for r in rows:
            self.subjects_tree.insert("", "end", values=(r["id"], r["name"], r["credits"], r["semester"]))

    def get_subject_form_data(self) -> dict:
        return {"name": self.sub_name.get(), "credits": self.sub_credits.get(), "semester": self.sub_semester.get()}

    def clear_subject_form(self) -> None:
        self.sub_name.set(""); self.sub_credits.set("3"); self.sub_semester.set("1")

    def get_selected_subject_id(self) -> int | None:
        sel = self.subjects_tree.selection()
        if not sel:
            return None
        return int(self.subjects_tree.item(sel[0], "values")[0])

    # ---------- Grades tab ----------
    def _build_grades_tab(self) -> None:
        left = ttk.Frame(self.tab_grades)
        left.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)

        self.grades_tree = ttk.Treeview(
            left, columns=("id","date","student","subject","type","value","ects","five"), show="headings", height=16
        )
        columns = [
            ("id","ID",60), ("date","Дата",110), ("student","Студент",240), ("subject","Предмет",220),
            ("type","Тип",80), ("value","100",60), ("ects","ECTS",70), ("five","5",50)
        ]
        for col, title, w in columns:
            self.grades_tree.heading(col, text=title)
            self.grades_tree.column(col, width=w, anchor=tk.CENTER if col in ("id","date","type","value","ects","five") else tk.W)
        self.grades_tree.pack(fill=tk.BOTH, expand=True)

        right = ttk.Frame(self.tab_grades, padding=(10,0,0,0))
        right.pack(side=tk.RIGHT, fill=tk.Y)

        self.gr_student_id = tk.StringVar()
        self.gr_subject_id = tk.StringVar()
        self.gr_type = tk.StringVar(value="exam")
        self.gr_value = tk.StringVar()
        self.gr_date = tk.StringVar()  # YYYY-MM-DD

        ttk.Label(right, text="Додати оцінку", font=("Segoe UI", 11, "bold")).pack(anchor="w", pady=(0,8))
        ttk.Label(right, text="Student ID").pack(anchor="w")
        ttk.Entry(right, textvariable=self.gr_student_id, width=32).pack(anchor="w", pady=(0,8))
        ttk.Label(right, text="Subject ID").pack(anchor="w")
        ttk.Entry(right, textvariable=self.gr_subject_id, width=32).pack(anchor="w", pady=(0,8))
        ttk.Label(right, text="Тип").pack(anchor="w")
        ttk.Combobox(right, textvariable=self.gr_type, values=("current","module","exam","final"), state="readonly", width=29).pack(anchor="w", pady=(0,8))
        ttk.Label(right, text="Оцінка (0..100)").pack(anchor="w")
        ttk.Entry(right, textvariable=self.gr_value, width=32).pack(anchor="w", pady=(0,8))
        ttk.Label(right, text="Дата (YYYY-MM-DD, опційно)").pack(anchor="w")
        ttk.Entry(right, textvariable=self.gr_date, width=32).pack(anchor="w", pady=(0,12))

        ttk.Button(right, text="Додати", command=lambda: self.grades_presenter.create()).pack(fill=tk.X, pady=(0,6))
        ttk.Button(right, text="Вилучити обрану", command=lambda: self.grades_presenter.delete_selected()).pack(fill=tk.X)

    def show_grades(self, rows: Sequence[dict]) -> None:
        for i in self.grades_tree.get_children():
            self.grades_tree.delete(i)
        for r in rows:
            self.grades_tree.insert("", "end", values=(
                r["id"], r["date"], r["student"], r["subject"], r["type"], r["value"], r["ects"], r["five"]
            ))

    def get_grade_form_data(self) -> dict:
        return {
            "student_id": self.gr_student_id.get(),
            "subject_id": self.gr_subject_id.get(),
            "grade_type": self.gr_type.get(),
            "value": self.gr_value.get(),
            "grade_date": self.gr_date.get(),
        }

    def clear_grade_form(self) -> None:
        self.gr_student_id.set(""); self.gr_subject_id.set(""); self.gr_type.set("exam"); self.gr_value.set(""); self.gr_date.set("")

    def get_selected_grade_id(self) -> int | None:
        sel = self.grades_tree.selection()
        if not sel:
            return None
        return int(self.grades_tree.item(sel[0], "values")[0])

    # ---------- Common ----------
    def show_message(self, text: str, level: str = "info") -> None:
        if level == "error":
            messagebox.showerror("Помилка", text)
        elif level == "success":
            messagebox.showinfo("Успіх", text)
        else:
            messagebox.showinfo("Інформація", text)

    def confirm(self, text: str) -> bool:
        return messagebox.askyesno("Підтвердження", text)
