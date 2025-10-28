from __future__ import annotations
from typing import Protocol, Optional
# from dataclasses import dataclass, field
# from datetime import datetime
from patterns._3_3_1_chain import FailedResponse, Request, Response


class IPipelineHandler (Protocol):
    def handle(self, request: Request) -> Response: ...


class PipelineManager:
    def __init__(self, max_handlers_count: int) -> None:
        self.max_handlers_count: int = max_handlers_count
        self._handlers: list[Optional[IPipelineHandler]] = [
            None for _ in range(max_handlers_count)]

    def set_handler(self, index: int, handler: IPipelineHandler) -> bool:
        if index < 0 or index > self.max_handlers_count:
            return False
        self._handlers[index] = handler
        return True

    def remove_handler(self, index: int) -> bool:
        if index < 0 or index > self.max_handlers_count or self._handlers[index] is None:
            return False
        self._handlers[index] = None
        return True

    def handle(self, request: Request) -> Response:
        response: Response = Response(
            value="No handlers",
            request=request
        )
        for handler in self._handlers:
            if handler is None:
                continue
            response = handler.handle(request)
            if type(response) is FailedResponse:
                break
        return response


class PipelineLogHandler:  # (IPipelineHandler)
    def handle(self, request: Request) -> Response:
        print(f"Log request: {request}")
        return Response(request=request)


class PipelineAuthorizeHandler:  # (IPipelineHandler)
    def check(self, login: str, password: str) -> bool:
        return login == "admin" and password == "admin"

    def handle(self, request: Request) -> Response:
        print("Authorize")
        if not self.check(request.login, request.password):
            print("Wrong login or password")
            return FailedResponse("Wrong login or password")
        return Response(request=request)


class PipelineResponseHandler:  # (IPipelineHandler)
    def handle(self, request: Request) -> Response:
        print("Response")
        return Response(
            request=request,
            value="42"
        )


class PipelineIncHandler:  # (IPipelineHandler)
    def handle(self, request: Request) -> Response:
        print(f"Inc Count")
        request.count += 1
        return Response(request=request)


class PipelineRoleHandler:  # (IPipelineHandler)
    def check(self, role: str) -> bool:
        return role == "admin"

    def handle(self, request: Request) -> Response:
        print(f"Role check")
        if not self.check(request.role):
            return FailedResponse(f"For {request.role} access denied")
        return Response(request=request)
