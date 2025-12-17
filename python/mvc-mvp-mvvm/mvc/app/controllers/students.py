from __future__ import annotations
from flask import Blueprint, render_template, request, redirect, url_for, flash
from ..extensions import db
from ..models.student import Student

bp = Blueprint("students", __name__)

@bp.get("/")
def list_students():
    students = Student.query.order_by(Student.full_name.asc()).all()
    return render_template("students/list.html", students=students)

@bp.get("/new")
def new_student_form():
    return render_template("students/form.html", student=None)

@bp.post("/new")
def create_student():
    full_name = (request.form.get("full_name") or "").strip()
    group = (request.form.get("group") or "").strip()
    year = (request.form.get("enrollment_year") or "").strip()

    if not full_name or not group or not year.isdigit():
        flash("Заповніть коректно ПІБ, групу та рік вступу.", "error")
        return redirect(url_for("students.new_student_form"))

    s = Student(full_name=full_name, group=group, enrollment_year=int(year))
    db.session.add(s)
    db.session.commit()
    flash("Студента додано.", "success")
    return redirect(url_for("students.list_students"))

@bp.get("/<int:student_id>")
def student_detail(student_id: int):
    student = Student.query.get_or_404(student_id)
    return render_template("students/detail.html", student=student)

@bp.get("/<int:student_id>/edit")
def edit_student_form(student_id: int):
    student = Student.query.get_or_404(student_id)
    return render_template("students/form.html", student=student)

@bp.post("/<int:student_id>/edit")
def update_student(student_id: int):
    student = Student.query.get_or_404(student_id)

    student.full_name = (request.form.get("full_name") or "").strip()
    student.group = (request.form.get("group") or "").strip()
    year = (request.form.get("enrollment_year") or "").strip()

    if not student.full_name or not student.group or not year.isdigit():
        flash("Некоректні дані.", "error")
        return redirect(url_for("students.edit_student_form", student_id=student_id))

    student.enrollment_year = int(year)
    db.session.commit()
    flash("Дані оновлено.", "success")
    return redirect(url_for("students.student_detail", student_id=student_id))

@bp.post("/<int:student_id>/delete")
def delete_student(student_id: int):
    student = Student.query.get_or_404(student_id)
    db.session.delete(student)
    db.session.commit()
    flash("Студента вилучено.", "success")
    return redirect(url_for("students.list_students"))
