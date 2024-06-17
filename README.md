# 2D-action
Unityでゲームの個人開発をおこないました。  
内容は、騎士がドラゴンの炎をよけながら、障害物をジャンプで乗り越え、剣でドラゴンを倒してゴールを目指す2Dアクションゲームです。  
[Unityroom](https://unityroom.com/games/knights_clash_battle_against_the_dragon)からプレイ可能です。
プロジェクトファイルとプレイ動画は[GoogleDrive]()から入手可能です。
本リポジトリから実行ファイルを入手した場合は、2Daction_Dataのzipファイルを解凍してください。  
  
操作方法　←, →　移動、↑　ジャンプ、↓　前転、space　剣で攻撃  

# 開発目的
ゲームを0から自分で制作することで、ゲーム開発全体の流れを理解すること  
将来自分がゲーム開発に携わった際に、ユーザーが求めているものやチームとして目指しているものを実現するための技術を身に着けること  
クオリティの高いゲーム開発おこなうために、必要な技術を学ぶこと  

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

