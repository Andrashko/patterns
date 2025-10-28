from __future__ import annotations
from typing import Protocol, Optional
from dataclasses import dataclass, field
from datetime import datetime


@dataclass
class Request:
    login: str
    password: str
    role: str = "user"
    count: int = 0
    created: datetime = field(default_factory=datetime.now)


@dataclass
class Response:
    request: Optional[Request] = None
    value: str = ""


class FailedResponse(Response):
    def __init__(self, message: str) -> None:
        super().__init__(request=None, value=f"Failed {message}")


class IHandler(Protocol):
    def set_next(self, handler: IHandler) -> IHandler: ...
    def handle(self, request: Request) -> Response: ...


class AbstractHandler:  # (IHandler)
    def __init__(self) -> None:
        self._next_handler: Optional[IHandler] = None

    def set_next(self, handler: IHandler) -> IHandler:
        self._next_handler = handler
        return handler

    def handle(self, request: Request) -> Response:
        if self._next_handler is None:
            return FailedResponse("no next handler")
        return self._next_handler.handle(request)


class LogHandler (AbstractHandler):
    def handle(self, request: Request) -> Response:
        print(f"Log request: {request}")
        return super().handle(request)


class AuthorizeHandler (AbstractHandler):
    def check(self, login: str, password: str) -> bool:
        return login == "admin" and password == "admin"

    def handle(self, request: Request) -> Response:
        print("Authorize")
        if not self.check(request.login, request.password):
            print("Wrong login or password")
            return FailedResponse("Wrong login or password")
        return super().handle(request)


class ResponseHandler (AbstractHandler):
    def handle(self, request: Request) -> Response:
        print("Response")
        return Response(
            request=request,
            value="42"
        )


class IncHandler (AbstractHandler):
    def handle(self, request: Request) -> Response:
        print(f"Inc Count")
        request.count += 1
        return super().handle(request)


class RoleHandler(AbstractHandler):
    def check(self, role: str) -> bool:
        return role == "admin"

    def handle(self, request: Request) -> Response:
        print(f"Role check")
        if not self.check(request.role):
            return FailedResponse(f"For {request.role} access denied")
        return super().handle(request)
