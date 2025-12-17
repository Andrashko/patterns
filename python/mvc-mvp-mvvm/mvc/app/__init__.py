from __future__ import annotations
from flask import Flask
from .config import Config
from .extensions import db, migrate

def create_app() -> Flask:
    app = Flask(__name__, instance_relative_config=True)
    app.config.from_object(Config)

    # init extensions
    db.init_app(app)
    migrate.init_app(app, db)

    # import models (important for migrations)
    from .models.student import Student  # noqa: F401
    from .models.subject import Subject  # noqa: F401
    from .models.grade import Grade      # noqa: F401

    # register blueprints
    from .controllers.home import bp as home_bp
    from .controllers.students import bp as students_bp
    from .controllers.subjects import bp as subjects_bp
    from .controllers.grades import bp as grades_bp

    app.register_blueprint(home_bp)
    app.register_blueprint(students_bp, url_prefix="/students")
    app.register_blueprint(subjects_bp, url_prefix="/subjects")
    app.register_blueprint(grades_bp, url_prefix="/grades")

    return app
