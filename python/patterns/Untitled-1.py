from __future__ import annotations
from typing import Any


# ==============================
# Базовий клас шахової фігури
# ==============================
class ChessPiece:
    def __init__(self, color: str, position: str):
        self.color = color
        self.position = position 

    def display(self) -> str:
        raise NotImplementedError("Метод display має бути перевизначений")


# ==============================
# Фабричний метод 
# ==============================
class ChessPieceFactory:
    _registry: dict[str, type[ChessPiece & ChessPieceFactory] ] = {}

    def __init_subclass__(cls, **kwargs: Any) -> None:
        super().__init_subclass__(**kwargs)
        if hasattr(cls, "kind"):
            ChessPieceFactory._registry[cls.kind] = cls

    @classmethod
    def create(cls, kind: str, color: str, position: str) -> ChessPiece:
        try:
            piece_cls = cls._registry[kind]
            return piece_cls(color, position)
        except KeyError:
            raise ValueError(f"Unknown piece kind: {kind}")


# ==============================
# Текстові фігури 
# ==============================
class TextPawn(ChessPiece, ChessPieceFactory):
    kind = "pawn_text"

    def display(self) -> str:
        return "P" if self.color == "white" else "p"


class TextRook(ChessPiece, ChessPieceFactory):
    kind = "rook_text"

    def display(self) -> str:
        return "R" if self.color == "white" else "r"


class TextKnight(ChessPiece, ChessPieceFactory):
    kind = "knight_text"

    def display(self) -> str:
        return "N" if self.color == "white" else "n"


class TextBishop(ChessPiece, ChessPieceFactory):
    kind = "bishop_text"

    def display(self) -> str:
        return "B" if self.color == "white" else "b"


class TextQueen(ChessPiece, ChessPieceFactory):
    kind = "queen_text"

    def display(self) -> str:
        return "Q" if self.color == "white" else "q"


class TextKing(ChessPiece, ChessPieceFactory):
    kind = "king_text"

    def display(self) -> str:
        return "K" if self.color == "white" else "k"


# ==============================
# Unicode фігури 
# ==============================
class UnicodePawn(ChessPiece, ChessPieceFactory):
    kind = "pawn_unicode"

    def display(self) -> str:
        return "♙" if self.color == "white" else "♟"


class UnicodeRook(ChessPiece, ChessPieceFactory):
    kind = "rook_unicode"

    def display(self) -> str:
        return "♖" if self.color == "white" else "♜"


class UnicodeKnight(ChessPiece, ChessPieceFactory):
    kind = "knight_unicode"

    def display(self) -> str:
        return "♘" if self.color == "white" else "♞"


class UnicodeBishop(ChessPiece, ChessPieceFactory):
    kind = "bishop_unicode"

    def display(self) -> str:
        return "♗" if self.color == "white" else "♝"


class UnicodeQueen(ChessPiece, ChessPieceFactory):
    kind = "queen_unicode"

    def display(self) -> str:
        return "♕" if self.color == "white" else "♛"


class UnicodeKing(ChessPiece, ChessPieceFactory):
    kind = "king_unicode"

    def display(self) -> str:
        return "♔" if self.color == "white" else "♚"


# ==============================
# Шахова дошка
# ==============================
class ChessBoard:
    def __init__(self):
       
        self.board: list[list[ChessPiece | None]] = [
            [None for _ in range(8)] for _ in range(8)
        ]

    def place_piece(self, piece: ChessPiece):
        col_map = {"a": 0, "b": 1, "c": 2, "d": 3,
                   "e": 4, "f": 5, "g": 6, "h": 7}
        col = col_map[piece.position[0]]
        row = 8 - int(piece.position[1])  
        self.board[row][col] = piece

    def setup(self, pieces: list[tuple[str, str, str]], factory: type[ChessPieceFactory]) -> ChessBoard:
        """я зробила тут метод що приймає список (тип, колір, позиція) і фабрику, створює фігури та розставляє їх."""
        for kind, color, position in pieces:
            piece = factory.create(kind, color, position)
            self.place_piece(piece)
        return self

    def display(self):
        print("  a b c d e f g h")
        for i, row in enumerate(self.board):
            print(f"{8 - i} ", end="")
            print(" ".join(piece.display() if piece else "." for piece in row))
        print()


# ==============================
# Приклад використання
# ==============================
if __name__ == "__main__":
    
    factory = ChessPieceFactory


    pieces = [
        ("rook_unicode", "white", "a1"),
        ("knight_unicode", "white", "b1"),
        ("bishop_unicode", "white", "c1"),
        ("queen_unicode", "white", "d1"),
        ("king_unicode", "white", "e1"),
        ("pawn_unicode", "white", "a2"),
        ("rook_unicode", "black", "a8"),
        ("king_unicode", "black", "e8"),
    ]

    board = ChessBoard().setup(pieces, factory)
    board.display()