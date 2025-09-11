from __future__ import annotations
from typing import Any, ClassVar

from patterns._1_2_1_classic_fabric_method import Application, WordApp, PDFApp


class AutoRegisterApplicationFabricMethod(Application):
    _registry: dict[str, type[Application]] = {}
    kind: ClassVar[str]

    def __init_subclass__(cls, **kwargs: Any) -> None:
        super().__init_subclass__(**kwargs)
        AutoRegisterApplicationFabricMethod._registry[cls.kind] = cls

    @classmethod
    def create(cls, kind: str, **kwargs: Any) -> Application:
        try:
            app_cls: type[Application] = cls._registry[kind]
            return app_cls(**kwargs)
        except KeyError:
            raise ValueError(f"Unknown kind: {kind}")


class AutoRegisterWordApp (WordApp, AutoRegisterApplicationFabricMethod):
    kind = "word"


class AutoRegisterPDFApp (PDFApp, AutoRegisterApplicationFabricMethod):
    kind = "pdf"
