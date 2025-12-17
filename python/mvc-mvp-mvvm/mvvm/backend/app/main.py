from __future__ import annotations

from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from sqlalchemy import text
from sqlalchemy.exc import OperationalError

from .api.router import router
from .core.config import settings
from .core.db import engine


app = FastAPI(title="Student Grades API")

app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5173", "http://127.0.0.1:5173"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

app.include_router(router)


@app.get("/health")
def health():
    # Швидко показати, куди підключились, і чи читається БД.
    try:
        with engine.connect() as conn:
            conn.execute(text("SELECT 1"))
    except OperationalError as e:
        return {
            "ok": False,
            "db_path": str(settings.db_path),
            "error": str(e),
        }
    return {"ok": True, "db_path": str(settings.db_path)}
