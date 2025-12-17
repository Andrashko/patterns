from __future__ import annotations
from dataclasses import dataclass
from pathlib import Path


@dataclass(frozen=True)
class Settings:

    BASE_DIR: Path = Path(__file__).resolve().parent.parent.parent.parent.parent

    @property
    def database_url(self) -> str:
        return f"sqlite:///{self.BASE_DIR / 'example.db'}"


settings = Settings()
