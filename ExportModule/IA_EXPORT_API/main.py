from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from dotenv import load_dotenv
import os
from agno.agent import Agent
from agno.models.groq import Groq
from DTO import *
import re
import json


# Cargar .env
load_dotenv()

# Configurar el agente
model_id = os.getenv("GROQ_MODEL_ID", "llama-3.3-70b-versatile")
agent = Agent(
    model=Groq(id=model_id),
    markdown=True
)

app = FastAPI()

class ChatRequest(BaseModel):
    prompt: str

@app.post("/chat")
def chat(request: ChatRequest):

    try:
        response = agent.run(request.prompt)
        return {"response": response.content}
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))



@app.post("/evaluar-cultivo")
def evaluar_cultivo(data: CultivoRequest):
    prompt_instruccion = """
    Tu función es determinar si un cultivo es apto para exportación basado en su estado sanitario.

Debes revisar si tiene plagas asociadas y qué nivel de severidad presentan. Las reglas básicas son:

1. Si alguna plaga tiene nivel crítico, el cultivo no es exportable.
2. Si todas las plagas tienen nivel moderado o leve, el cultivo puede exportarse.
3. Si no hay plagas, el cultivo también es exportable.

Devuelve la decisión en el siguiente formato JSON:
{
  "apto_para_exportacion": true | false,
  "motivo": "explicación breve y clara"
}
"""

    cultivo_json = json.dumps(data.dict(), ensure_ascii=False, indent=2)
    full_prompt = f"{prompt_instruccion}\n\nEstos son los datos que debes evaluar:\n{cultivo_json}"

    try:
        response = agent.run(full_prompt)
        content = response.content


        # Busca el último bloque JSON dentro de triple backticks o sin ellos
        matches = re.findall(r'```json\s*(\{[\s\S]*?\})\s*```|(\{[\s\S]*?"apto_para_exportacion"[\s\S]*?\})', content)

        # Selecciona el primer match no vacío
        json_str = None
        for m in matches:
            json_str = m[0] or m[1]
            if json_str:
                break

        if json_str:
            try:
                resultado_dict = json.loads(json_str)
                return resultado_dict
            except json.JSONDecodeError as e:
                raise HTTPException(status_code=500, detail=f"Error al decodificar JSON: {str(e)}")
        else:
            raise HTTPException(status_code=500, detail="No se encontró un JSON válido en la respuesta.")

    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

