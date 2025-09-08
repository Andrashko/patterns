from copy import deepcopy
from datetime import datetime
from random import randint
from typing import Self
from dataclasses import dataclass, field


@dataclass
class InnerObjectToCopy:
    name: str = "No name"
    count: int = 0


@dataclass
class ObjectToCopy:
    id: int = randint(0, 1_000_000)
    created_at: datetime = field(default_factory=datetime.now)
    inner_object: InnerObjectToCopy = field(default_factory=InnerObjectToCopy)

    def __deepcopy__(self, memo: dict) -> Self:
        return type(self)(
            id=self.id,
            created_at=datetime.now(),
            inner_object=deepcopy(self.inner_object, memo)
        )
