## 🚀 Fase 1 — Heurística antes do código (mapa mental)

### ✅ Objetivo da Fase
- Enunciado: Desenhe um mapa de evolução para um problema trivial escolhido pela equipe.
- Descrição: Uma página com: (1) versão procedural (onde surgem if/switch ), (2) OO sem interface
(quem muda o quê), (3) com interface (qual contrato permite alternar). Liste 3 sinais de alerta
previstos.

---

### ✅ Descrição dos passos seguidos

### Quadro 1 — Abordagem Procedural (if/switch)

- Fluxo típico: receber solicitação → verificar tipo → autenticar → retornar resultado.
- Cada novo método exige novos if/switch.
- Problema: código se torna difícil de manter e testar.

---

### Quadro 2 — OO sem Interface

- Criamos classes específicas: BiometriaAuthenticator, CartaoSenhaAuthenticator.
- Um controlador central decide qual classe usar.
- Melhora: lógica separada por método.
- Limitação: cliente continua acoplado às classes concretas.

---

### Quadro 3 — OO com Interface (Desacoplamento)

- Criamos um contrato: IAuthenticator (método autenticar()).
- Implementações: biometria, cartão/senha, token, RFID, etc.
- Ponto de composição: seleção do método por configuração, fábrica ou injeção de dependência.
- Resultado: cliente não muda ao adicionar novos autenticadores; testes ficam mais simples.

---

### Sinais de alerta a evitar:
- Cliente mudando ao trocar implementação.
- if/switch espalhados.
- Testes que dependem de dispositivos reais.

---