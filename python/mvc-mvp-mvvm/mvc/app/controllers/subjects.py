from __future__ import annotations
from flask import Blueprint, render_template, request, redirect, url_for, flash
from sqlalchemy.exc import IntegrityError
from ..extensions import db
from ..models.subject import Subject

bp = Blueprint("subjects", __name__)

@bp.get("/")
def list_subjects():
    subjects = Subject.query.order_by(Subject.name.asc()).all()
    return render_template("subjects/list.html", subjects=subjects)

@bp.get("/new")
def new_subject_form():
    return render_template("subjects/form.html", subject=None)

@bp.post("/new")
def create_subject():
    name = (request.form.get("name") or "").strip()
    credits = (request.form.get("credits") or "").strip()
    semester = (request.form.get("semester") or "").strip()

    if not name or not credits.isdigit() or not semester.isdigit():
        flash("Некоректні дані предмета.", "error")
        return redirect(url_for("subjects.new_subject_form"))

    s = Subject(name=name, credits=int(credits), semester=int(semester))
    db.session.add(s)
    try:
        db.session.commit()
    except IntegrityError:
        db.session.rollback()
        flash("Предмет з такою назвою вже існує.", "error")
        return redirect(url_for("subjects.new_subject_form"))

    flash("Предмет додано.", "success")
    return redirect(url_for("subjects.list_subjects"))
