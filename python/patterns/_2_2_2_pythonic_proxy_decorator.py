from __future__ import annotations
from patterns._2_1_proxy import IRequest, RequestManager
from typing import TypeVar, Type, Protocol, Callable, cast
from functools import wraps
from random import random

T = TypeVar("T", bound=IRequest)


class ProxyAugmented(Protocol):
    def check_access(self) -> bool: ...
    def log_access(self, message: str) -> None: ...


def proxy_request(cls: Type[T]) -> Type[T]:
    # додаємо методи динамічно
    def check_access(self: IRequest) -> bool:
        return random() < 0.5

    def log_access(self: IRequest, message: str) -> None:
        print(f"Request was handle by proxy: {message}")

    setattr(cls, "check_access", check_access)
    setattr(cls, "log_access", log_access)

    # type: ignore[attr-defined]
    original_request: Callable[[T], str] = cls.request

    @wraps(original_request)
    def wrapped(self: T) -> str:
        # статично звужуємо до протоколу з потрібними методами
        s = cast(ProxyAugmented, self)
        if not s.check_access():
            return "Proxy response"
        response: str = original_request(self)
        s.log_access(response)
        return response

    setattr(cls, "request", wrapped)
    return cls


@proxy_request
class RequestManagerPythonicProxy(RequestManager):
    ...
