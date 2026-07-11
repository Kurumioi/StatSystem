# GAS 编辑器功能清单（草案）

> 用途：读完源码后 勾选你想做的功能  
> 分工约定：**你只负责把运行时系统搞好 编辑器实现交给 AI 不考虑制作成本**  
> 勾选标准：看「对理解 / 配置正确性 / 运行时调试」有没有帮助 不看工期  
> 对照源码：`Assets/_Scripts/GAS/`  
> 配套阅读：`GAS源码阅读顺序.md`

勾选约定：

- `[ ]` 未定
- `[x]` 要做
- `[-]` 明确不做 / 以后再说

优先级建议（与工期无关 只表示对系统的价值）：

| 标记 | 含义 |
|------|------|
| P0 | 直接降低配错率 / 理解成本 强烈建议要 |
| P1 | 明显提升配置与调试效率 建议要 |
| P2 | 锦上添花 资产多了或多人用时很香 |
| P3 | 依赖尚未存在的运行时能力 等内核扩展后再说 |

---

## 0. 总览：编辑器可能长什么样

```text
┌─────────────────────────────────────────────────────────┐
│  GAS Editor                          [刷新] [定位选中]   │
├──────────┬──────────────────────────────────────────────┤
│ 导航     │  主工作区                                     │
│          │                                              │
│ Tag      │  树 / 列表 / 表单 / 预览 / 校验结果            │
│ Stat     │                                              │
│ GE       │                                              │
│ Ability  │                                              │
│ ASC调试  │                                              │
│ 校验中心 │                                              │
│ 设置     │                                              │
└──────────┴──────────────────────────────────────────────┘
```

可以一次勾很多 甚至全要。落地时仍建议按「依赖关系」排期（不是因为贵 而是先有数据源再有表单再有调试）：

1. Tag Browser（P0）— 其它选择器都依赖它
2. 资产浏览器 + 一键创建（P0/P1）
3. GE / Ability 可视化表单（P1）
4. Play 模式 ASC 调试（P1）— 最能反哺运行时正确性
5. 全局校验 / 引用分析（P2）

---

## 1. Tag 模块编辑器

对应源码：

- `Core/TagSystem/GameplayTag.cs`
- `Core/TagSystem/GameplayTagContainer.cs`
- `Core/TagSystem/GameplayTagCondition.cs`
- `Core/TagSystem/GameplayTagDatabase.cs`

### 1.1 Tag Browser（标签浏览器）

| 功能 | 说明 | 优先级 | 选用 |
|------|------|--------|------|
| 打开 Database | 自动定位 `Resources/GAS/GameplayTagDatabase` | P0 | [ ] |
| 列表 / 树形展示 | 按 `.` 拆层级 如 `State` → `State.Stunned` | P0 | [ ] |
| 搜索过滤 | 按名字模糊搜 | P0 | [ ] |
| 新增标签 | 输入完整路径写入 Database | P0 | [ ] |
| 重命名标签 | 改名并可选批量替换引用资产 | P1 | [ ] |
| 删除标签 | 删除前检查被哪些 GE/Ability/ASC 引用 | P1 | [ ] |
| 复制标签名 | 一键复制到剪贴板 | P0 | [ ] |
| 父子关系预览 | 选中 Tag 显示其父/子集合 | P1 | [ ] |
| 非法字符校验 | 空格、重复、空段（`A..B`）提示 | P0 | [ ] |

### 1.2 Tag 选择器增强（可替代纯 ValueDropdown）

| 功能 | 说明 | 优先级 | 选用 |
|------|------|--------|------|
| 自定义 Tag 字段 Drawer | Inspector 里弹树形选择 而不是裸下拉 | P1 | [ ] |
| Need / Ban 双栏编辑 | 对应 `GameplayTagCondition` 语义可视化 | P1 | [ ] |
| 多选 Tag 面板 | 给 `string[]` 用的批量勾选 | P1 | [ ] |

> 备注：若做了好用的 Tag Drawer ValueDropdown 可保留作兜底。

---

## 2. Stat 模块编辑器

对应源码：

- `Core/StatSystem/Data/StatData.cs`
- `Core/StatSystem/StatController.cs`
- `Core/StatSystem/Runtime/PassiveStat.cs`
- `Core/StatSystem/Data/ImmediateStat.cs`
- `Core/StatSystem/Data/StatModifier.cs`

| 功能 | 说明 | 优先级 | 选用 |
|------|------|--------|------|
| Stat 资产列表 | 扫描项目内所有 `StatData` | P0 | [x] |
| 快速创建 Stat | Passive / Immediate 模板一键生成 | P0 | [x] |
| 类型筛选 | 只看被动 / 只看即时 | P1 | [x] |
| 基础值 / 上下限预览 | 不打开 SO 也能扫一眼 | P1 | [x] |
| StatId 下拉统一源 | GE 修饰符、Ability 消耗共用同一 Stat 列表 | P1 | [x] |
| 未使用 Stat 检测 | 没有任何引用的 StatData | P2 | [ ] |
| 修饰符阶段说明面板 | 解释 Additive / Multiplicative 等计算顺序 | P2 | [x] |

---

## 3. GameplayEffect（GE）模块编辑器

对应源码：

- `Core/Effect/GameplayEffectData.cs`
- `Core/Effect/GameplayEffectEnums.cs`
- `Core/Effect/GameplayEffectSpec.cs`
- `Core/Effect/GEManager.cs`

### 3.1 GE 资产工作台

| 功能 | 说明 | 优先级 | 选用 |
|------|------|--------|------|
| GE 资产列表 | 搜索 / 按命名筛选 | P0 | [ ] |
| 一键创建 GE | Instant / Duration / Infinite / Dot 模板 | P0 | [ ] |
| 可视化表单 | 分组编辑：Tag / 时间 / 周期 / 堆叠 / 修饰符 | P1 | [ ] |
| 策略互斥提示 | 如 Instant 时隐藏 Duration/Stack 无意义项 | P1 | [ ] |
| 修饰符编辑器 | `statId` 下拉 + type + value + priority | P1 | [ ] |
| 授予 Tag 预览 | 显示该 GE 会给目标挂哪些 Tag | P1 | [ ] |
| Need/Ban 条件预览 | 应用前要满足什么 | P1 | [ ] |
| 复制为新 GE | Duplicate 并自动改名 | P1 | [ ] |

### 3.2 GE 语义辅助（降低理解成本）

| 功能 | 说明 | 优先级 | 选用 |
|------|------|--------|------|
| 人话摘要 | 例：「持续 5 秒 每 1 秒扣血 最多叠 3 层」 | P1 | [ ] |
| 枚举说明气泡 | Stacking / Refresh / Expiration 策略解释 | P2 | [ ] |
| 模板库 | 伤害、治疗、中毒 Dot、减速、攻击 Buff 预设 | P1 | [ ] |

### 3.3 GE 运行时调试（需 Play）

| 功能 | 说明 | 优先级 | 选用 |
|------|------|--------|------|
| 查看已应用 GE 列表 | 对接 `GEManager` 当前 `appliedEffectList` | P1 | [ ] |
| 剩余时间 / 层数 | 看 Spec 倒计时与 stack | P1 | [ ] |
| 手动 Apply / Remove | 选中目标 ASC 测试 GE | P1 | [ ] |
| Tag 引用计数查看 | 对应 `gameplayTagCountDict` | P2 | [ ] |

---

## 4. Ability 模块编辑器

对应源码：

- `Core/AbilitySystem/GameplayAbility.cs`
- `Core/AbilitySystem/AbilitySpec.cs`
- `Core/AbilitySystem/AbilityCost.cs`
- `Core/AbilitySystem/AbilityCooldown.cs`
- `Core/AbilitySystem/AbilityContext.cs`

| 功能 | 说明 | 优先级 | 选用 |
|------|------|--------|------|
| Ability 资产列表 | 搜索 / 打开 / 创建 | P0 | [ ] |
| 基础信息编辑 | 图标 名称 描述 CD | P1 | [ ] |
| 消耗配置可视化 | costStatId 下拉 + 类型 + 数值 | P1 | [ ] |
| 激活 Tag 条件编辑 | Required / Blocked 双栏 | P1 | [ ] |
| CanActivate 检查清单预览 | CD / 消耗 / Tag 三项是否会拦 | P2 | [ ] |
| 子类 Ability 识别 | 列出 `MyAbility` 等继承类 | P2 | [ ] |
| 技能流程图（可选） | 激活 → 扣费 → 上 CD → 执行 示意 | P3 | [ ] |

---

## 5. ASC / 角色装配编辑器

对应源码：

- `Component/AbilitySystemComponent.cs`
- `Core/StatSystem/StatController.cs`
- `Core/Effect/GEManager.cs`

### 5.1 编辑态装配

| 功能 | 说明 | 优先级 | 选用 |
|------|------|--------|------|
| 选中角色看 ASC 配置 | 初始技能 / 初始 Tag / 依赖组件 | P1 | [ ] |
| 缺失组件检测 | 缺 StatController / GEManager 标红 | P0 | [ ] |
| 初始 Tag 可视化编辑 | 对接 Database | P1 | [ ] |
| 初始技能拖拽列表 | 管理 `initialAbilities` | P1 | [ ] |
| StatController 属性清单 | 该角色带了哪些 StatData | P1 | [ ] |

### 5.2 Play 模式调试面板（强烈建议后期做）

| 功能 | 说明 | 优先级 | 选用 |
|------|------|--------|------|
| 当前持有 Tag | 实时显示 `GameplayTagContainer` | P1 | [ ] |
| 当前属性值 | Base / Current / Final | P1 | [ ] |
| 当前 GE | 名称 剩余时间 层数 | P1 | [ ] |
| 可激活技能状态 | CD 中 / 可放 / Tag 不满足 | P1 | [ ] |
| 一键加 Tag / 清 Tag | 调试用 | P2 | [ ] |
| 一键放技能 | 调 `TryActivateAbility` | P2 | [ ] |
| 一键上 GE | 选 SO 应用到目标 | P2 | [ ] |

---

## 6. 全局校验与引用分析

跨模块 资产一多就很值钱。

| 功能 | 说明 | 优先级 | 选用 |
|------|------|--------|------|
| 无效 Tag 扫描 | GE/Ability/ASC 引用了 Database 里没有的 Tag | P1 | [ ] |
| 无效 StatId 扫描 | 修饰符/消耗指向不存在的 Stat | P1 | [ ] |
| 空配置警告 | 空数组、空 statId、Duration=0 等 | P2 | [ ] |
| 引用查找 | 某 Tag 被哪些资产使用 | P1 | [ ] |
| 批量修复入口 | 重命名 Tag 后一键替换 | P2 | [ ] |
| 导出报告 | Markdown / CSV 给策划看 | P3 | [ ] |

---

## 7. 工作流与工程向功能

| 功能 | 说明 | 优先级 | 选用 |
|------|------|--------|------|
| 统一创建菜单 | 在窗口内 Create Stat/GE/Ability/Tag | P0 | [ ] |
| 默认保存路径设置 | 新建资产落到指定文件夹 | P1 | [ ] |
| 命名规范检查 | 如 `GE_` `Stat_` 前缀 | P2 | [ ] |
| 文档内嵌 | 窗口内链到阅读顺序 / 本清单 | P2 | [ ] |
| 中英文切换 | 一般不需要 | P3 | [ ] |
| Odin 依赖封装 | 有 Odin 用更好 Drawer 无 Odin 也能跑 | P2 | [ ] |

---

## 8. 暂时可跳过的（不是因为贵 是因为没运行时支撑）

这些功能本身可以做 但现在做了也挂不到你现有内核上 等运行时扩展后再勾：

| 功能 | 原因 |
|------|------|
| 可视化技能蓝图节点图 | 当前 Ability 效果靠脚本重写 没有节点执行模型可对接 |
| 网络同步调试 | 项目未见联网 / 预测回滚层 |
| 完整 Timeline / 动画事件绑定器 | 属于表现层 不是当前 GAS 运行时职责 |
| Excel 导表全流程 | 需要先定表结构与导入约定 |
| 多 Database 合并冲突工具 | 当前单 Database 足够 多库策略未定 |

> 若你明确要往这些方向扩运行时 再把对应编辑器一起勾上即可。

---

## 9. 推荐分期（按依赖关系 不是按工期）

### 第一期：Tag 底座

- [ ] Tag Browser 树 + 增删 + 搜索
- [ ] 非法 Tag 校验
- [ ] 打开/保存 Database

目标：消灭「不懂 ValueDropdown 字符串」的痛点 给后续选择器供数。

### 第二期：资产导航

- [ ] Stat / GE / Ability 列表浏览
- [ ] 一键创建模板资产
- [ ] 缺失组件检测

目标：新人知道「东西在哪、怎么新建」。

### 第三期：配置体验

- [ ] GE 可视化表单 + 人话摘要
- [ ] Ability 条件/消耗可视化
- [ ] StatId / Tag 统一选择器

目标：少开一堆 Inspector 也能配完常见内容 降低配错。

### 第四期：运行时调试（对你最值钱）

- [ ] Play 模式 ASC 面板（Tag / Stat / GE / Ability）
- [ ] 手动 Apply GE / 放技能

目标：验证运行时行为 调 Bug 不用靠 Log 海。

### 第五期：校验中心

- [ ] 无效 Tag / StatId 扫描
- [ ] 引用查找与重命名替换

目标：资产膨胀后保底质量 改名不炸引用。

> 你不考虑制作成本的话：可以一期勾很多 甚至 1～5 全要。  
> AI 实现时仍建议按上面依赖顺序提交 避免「表单做好了却没有 Tag 数据源」。

---

## 10. 决策记录（读完源码后填）

| 问题 | 你的结论 |
|------|----------|
| 主要使用者是谁（自己 / 同学 / 策划） | |
| 当前最痛的是配置正确性还是运行时调试 | |
| 是否一次上全量工作台（Tag+Stat+GE+Ability+调试） | |
| 是否接受依赖 Odin 做编辑器 UI | |
| 有没有计划扩展的运行时能力（会影响第 8 节） | |

### 最终选用摘要

把上面勾了 `[x]` 的功能抄到这里 方便开工：

```text
要做：
- 

明确不做：
- 
```

---

## 11. 和现有方案的关系

| 现有能力 | 编辑器关系 |
|----------|------------|
| `GameplayTagDatabase` + `ValueDropdown` | 继续保留 编辑器是增强不是替换 |
| Odin Inspector 分组 | 单资产编辑仍可用 窗口负责「全局视角」 |
| `GAS源码阅读顺序.md` | 教「怎么读」 本清单教「做什么工具」 |
| Demo / UnitTest | 调试面板可复用测试思路 但 UI 化 |

一句话：**你专注运行时正确性 编辑器交给 AI。勾选只看「能不能帮你少配错、看清运行时」不看贵不贵。**
