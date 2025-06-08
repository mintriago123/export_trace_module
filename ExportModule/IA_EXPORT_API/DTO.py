from typing import List, Literal
from pydantic import BaseModel

class Plaga(BaseModel):
    id: int
    nombre: str
    nivel: Literal["leve", "moderado", "crítico"]

class Cultivo(BaseModel):
    id: int
    nombre: str
    tipo: str

class CultivoRequest(BaseModel):
    cultivo: Cultivo
    plagas: List[Plaga]
