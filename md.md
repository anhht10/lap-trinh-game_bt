                 PLAYER ATTACK
                      │
                      ▼
                ┌───────────┐
                │ DamageData│  ← đòn này có 50 damage,
                └─────┬─────┘     20% crit...
                      │
                      ▼
                ┌───────────┐
                │ Hurtbox   │  ← trúng đầu/tay/ngực...
                └─────┬─────┘
                      │
                      ▼
             ┌─────────────────┐
             │ Damage Calculator│
             └────────┬────────┘
                      │
             ┌────────┼─────────┐
             ▼        ▼         ▼
           Crit     Armor    Resistance
             │        │         │
             └────────┼─────────┘
                      ▼
                FINAL DAMAGE
                      │
                      ▼
                  ┌───────┐
                  │ Health│
                  └───────┘

---

                    ATTACK
                      │
                      ▼
                ┌───────────┐
                │ DamageData│
                └─────┬─────┘
                      │
                      ▼
                 Hit Detection
                      │
                      ▼
               ┌─────────────┐
               │   Shield    │
               │  Collider   │
               └──────┬──────┘
                      │
                Absorb Damage
                      │
                      ▼
              Remaining Damage
                      │
                      ▼
               Continue Raycast
                      │
                      ▼
               ┌─────────────┐
               │   Hurtbox   │
               └──────┬──────┘
                      │
                      ▼
             DamageCalculator
                      │
          ┌───────────┼───────────┐
          ▼           ▼           ▼
      Body Part     Critical    Armor
      Modifier      Modifier   Modifier
          │           │           │
          └───────────┼───────────┘
                      ▼
                 Final Damage
                      │
                      ▼
                   Health
