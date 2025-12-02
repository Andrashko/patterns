from dataclasses import dataclass
from typing import Protocol


@dataclass
class Item:
    name: str
    qty: int = 0


class ShoppingCart:
    def __init__(self) -> None:
        self.items: dict[int, Item] = {}

    def add_item(self, item_id: int, name: str, qty: int = 1):
        if item_id not in self.items:
            self.items[item_id] = Item(name=name, qty=0)
        self.items[item_id].qty += qty

    def remove_item(self, item_id: int, qty: int = 1):
        if item_id in self.items:
            self.items[item_id].qty -= qty
            if self.items[item_id].qty <= 0:
                del self.items[item_id]

    def __str__(self):
        return f"Cart: {self.items}"


class ICommand(Protocol):
    def execute(self, cart: ShoppingCart): ...


@dataclass
class _AddItemCommand:  # ICommand:
    item_id: int
    name: str
    qty: int = 1

    def execute(self, cart: ShoppingCart):
        cart.add_item(self.item_id, self.name, self.qty)


@dataclass
class _RemoveItemCommand:  # (_Command):
    item_id: int
    qty: int = 1

    def execute(self, cart: ShoppingCart):
        cart.remove_item(self.item_id, self.qty)


class CartHistory:
    def __init__(self, cart: ShoppingCart):
        self.cart = cart
        self._commands: list[ICommand] = []
        self._undo_stack: list[ICommand] = []

    # ==== ПУБЛІЧНИЙ API (видимий користувачу) ====

    def add_item(self, item_id: int, name: str, qty: int = 1) -> None:
        cmd = _AddItemCommand(item_id, name, qty)
        inverse = _RemoveItemCommand(item_id, qty)
        self._apply(cmd, inverse)

    def remove_item(self, item_id: int, qty: int = 1) -> None:
        # Для inverse-команди треба знати name на цей момент
        item = self.cart.items.get(item_id)
        if item is None:
            print(f"Error. Unable to delete {item_id}")
            return
        current_name = item.name
        cmd = _RemoveItemCommand(item_id, qty)
        inverse = _AddItemCommand(item_id, current_name, qty)
        self._apply(cmd, inverse)

    def undo(self) -> None:
        """Відмінити останню операцію через зворотну команду."""
        if not self._undo_stack:
            print("Nothing to undo.")
            return
        inverse_cmd = self._undo_stack.pop()
        inverse_cmd.execute(self.cart)
    # ==== ВНУТРІШНІЙ API ====

    def _apply(self, command: ICommand, inverse: ICommand):
        command.execute(self.cart)
        self._commands.append(command)
        self._undo_stack.append(inverse)
