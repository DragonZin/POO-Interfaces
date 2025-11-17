## 🚀 Fase 2 — Procedural mínimo (ex.: formatar texto)

### ✅ Objetivo da Fase
- Enunciado: Implemente a ideia de modos (mínimo 3 + padrão) para um objetivo simples.
- Descrição: Entregue função/fluxo e 5 cenários de teste/fronteira descritos em texto. Explique em
poucas linhas por que essa abordagem não escala.

---

### ✅ Descrição dos passos seguidos

#### Definição do objetivo
- Foi estabelecida a necessidade de registrar auditorias de acesso com diferentes níveis de detalhe.

#### Catalogação dos modos de registro
- Foram definidos quatro modos de operação:
  - Completo
  - Resumido
  - Crítico
  - Padrão (fallback automático)

#### Regras de decisão por modo
- Para cada modo, definimos o que deve ser registrado e como determinar o comportamento quando o modo não for reconhecido.

#### Desenho do fluxo procedural 
- Organizou-se a sequência operacional
  - receber solicitação → decidir o modo → registrar conforme a regra → retornar status.

#### Especificação de entradas e saídas
- Entradas necessárias:
  - Usuário
  - Recurso acessado
  - Operação executada
  - Modo de registro solicitado

- Saída:
  - Status da operação (sucesso/erro).

#### Definição de cenários de teste
- Foram planejados testes cobrindo:
  - Entrada mínima (dados vazios)
  - Limites de tamanho
  - Modo inválido
  - Casos envolvendo recursos sensíveis
  - Caso de uso comum esperado

#### Critério para recursos sensíveis
- Identificamos quando um recurso deve ser tratado como crítico para aplicar logs mais completos.

#### Fallback seguro
- Caso o modo informado seja inválido, o sistema deve automaticamente operar no modo Resumido.

#### Análise de escalabilidade
- Reconhecemos que o uso de condicionais (if/switch) torna o sistema pouco escalável. Novas regras exigem mais ramificações, aumentando complexidade e custos de manutenção.

#### Registro das decisões e próximos passos
- Concluímos os pontos críticos da fase e preparamos terreno para evoluir a solução para um modelo mais flexível (ex.: uso de abstrações/Strategy/Object-Oriented).

---