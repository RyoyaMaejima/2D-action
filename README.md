# 2D-action
Unityでゲームの個人開発をおこないました。  
内容は、騎士がドラゴンの炎をよけながら、障害物をジャンプで乗り越え、剣でドラゴンを倒してゴールを目指す2Dアクションゲームです。  
[Unityroom](https://unityroom.com/games/knights_clash_battle_against_the_dragon)からプレイ可能です。  
操作方法　←, →　移動、↑　ジャンプ、↓　前転、space　剣で攻撃  

# ディレクトリ構成
Script内にソースコード一式を載せています。  
以下が各ファイルの説明。  
  
Boss ボス  
BossAttack ボスの攻撃（炎）  
ClearEffect クリア演出の出力  
ClearFrame クリアポイントの周りに存在し、クリアすると消えるオブジェクト  
CoutinuePoint コンティニューポイント  
Enemy 敵  
EnemyAttack 敵の攻撃（炎）  
EnemyCollisionCheck 敵のコリジョン  
EnemyTriggerCheck 敵が前方にある壁の検知
FadeActiveUGUI メッセージの出力  
FadeImage フェード  
GameTime ゲーム時間の出力  
GManager ゲームマネージャー  
GroundCheck プレイヤーが地面についているかの判定  
Heart 残り残機の出力  
HP HPの出力  
MoveObject  動く床
ObjectCollision  プレイヤーが敵を踏んだ時の挙動  
Player  プレイヤー  
PlayerAttack  プレイヤーの攻撃（剣）  
PlayerCollisionCheck  プレイヤーのコリジョン  
PlayerTriggerCheck  プレイヤーの侵入判定  
Score スコアの出力  
ScoreItem 取るとスコアが上がるアイテム  
StageCtrl ステージコントローラー  
StageNum ステージ番号の出力  
Title タイトル画面  

# こだわった点
・プレイヤーの物理挙動を調整し、一定速度で落下するなど、操作がしやすいように工夫しました  
・プレイヤーや敵のスピード、攻撃する時間間隔などをパブリック変数にすることで、テストプレイ中に調整しやすいように工夫しました  
・ボスの攻撃手段に関して、sin関数とcos関数を用いて炎がボスの周りを回転するようにしました  

# プレイ動画

https://github.com/RyoyaMaejima/2D-action/assets/95085279/d53d43d0-c8b2-4552-9fc4-4524ce48f9ed


