# SecureGate – Sistema Integrado de Controle e Auditoria de Acessos

> Gestão integrada de controle de acesso e monitoramento de segurança em ambientes corporativos, envolvendo autenticação de usuários (física e digital) e registro auditável de eventos, a fim de garantir que apenas pessoas autorizadas acessem áreas e sistemas, além de permitir rastreabilidade e detecção de irregularidades.

---

## 👥 Composição da Equipe

**Equipe 7 — ISP Splitters**

| Nome Completo               | RA      | 
|-----------------------------|---------|
| Jeancarlo Soutes dos Santos | 2400626 |
| Hilário Canci Neto          | 2706016 |
| Enzo Mazzutti Trevisan      | 2454874 |

---

## 📌 Sumário

- [Fase 0 – Aquecimento conceitual](#-fase-0--aquecimento-conceitual-contratos-de-capacidade-sem-código-)
- [Fase 1 – Heurística antes do código](#-fase-1--heurística-antes-do-código-mapa-mental-)
- [Fase 2 — Procedural mínimo](#-fase-2--procedural-mínimo-ex-formatar-texto-)
- [Fase 3 — OO sem interface ](#-fase-3--oo-sem-interface-)
- [Fase 4 — Interface plugável e testável](#-fase-4--interface-plugável-e-testável-)

---

## 🚀 Fase 0 — Aquecimento conceitual: contratos de capacidade (sem código) [↗](src/fase-00-aquecimento)

### ✅ Objetivo da Fase
- Enunciado: Liste 2 situações reais com mesmo objetivo e peças alternáveis. Nomeie o contrato (o
que) e duas possíveis implementações (como).  
- Descrição: Refere-se ao aquecimento do guia. Em 4–6 linhas por caso: objetivo, contrato, duas peças e
uma política simples (ex.: “à noite usar A; em urgência, B”).

---

## 🚀 Fase 1 — Heurística antes do código (mapa mental) [↗](src/fase-01-heuristica)

### ✅ Objetivo da Fase
- Enunciado: Desenhe um mapa de evolução para um problema trivial escolhido pela equipe.
- Descrição: Uma página com: (1) versão procedural (onde surgem if/switch ), (2) OO sem interface
(quem muda o quê), (3) com interface (qual contrato permite alternar). Liste 3 sinais de alerta
previstos.

---

## 🚀 Fase 2 — Procedural mínimo (ex.: formatar texto) [↗](src/fase-02-procedural)

### ✅ Objetivo da Fase
- Enunciado: Implemente a ideia de modos (mínimo 3 + padrão) para um objetivo simples.
- Descrição: Entregue função/fluxo e 5 cenários de teste/fronteira descritos em texto. Explique em
poucas linhas por que essa abordagem não escala.

---

## 🚀 Fase 3 — OO sem interface [↗](src/fase-03-oo-sem-interface)

### ✅ Objetivo da Fase
- Enunciado: Transforme a solução anterior em uma hierarquia com variações concretas e base comum.
- Descrição: Substitua decisões por polimorfismo. Mantenha classes concretas restritas a sua
responsabilidade e descreva o que melhorou/ficou rígido.

---

## 🚀 Fase 4 — Interface plugável e testável [↗](src/fase-04-com-interfaces)

### ✅ Objetivo da Fase
- Enunciado: Defina um contrato claro e refatore o cliente para depender dele.
- Descrição: Explique como alternar implementações sem mudar o cliente e como dobrar a
dependência em testes (injeção simples).

---