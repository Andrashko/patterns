from __future__ import annotations
from typing import Any

from patterns._1_2_1_classic_fabric_method import Application, WordApp, PDFApp


class AutoRegisterApplicationFabricMethod:
    _registry: dict[str, Application] = {}

    def __init_subclass__(cls, **kwargs: Any) -> None:
        super().__init_subclass__(**kwargs)
        AutoRegisterApplicationFabricMethod._registry[cls.kind] = cls

    @classmethod
    def create(cls, kind: str, **kwargs: Any) -> AutoRegisterApplicationFabricMethod:
        try:
            return cls._registry[kind](**kwargs)
        except KeyError:
            raise ValueError(f"Unknown kind: {kind}")


class AutoRegisterWordApp (WordApp, AutoRegisterApplicationFabricMethod):
    kind = "word"


class AutoRegisterPDFApp (PDFApp, AutoRegisterApplicationFabricMethod):
    kind = "pdf"
