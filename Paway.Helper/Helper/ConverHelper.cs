using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 将一个基本数据类型转换为另一个基本数据类型。
    /// </summary>
    public static class ConverHelper
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 五笔编码字符
        /// </summary>
        private static readonly List<string> wbList = new List<string>(26);
        /// <summary>
        /// 初始化
        /// </summary>
        static ConverHelper()
        {
            wbList.Add("工式匿葚慝萁苷甙芽戒苣蕘荛節节蔭荫茸邛蒸菡蓀荪茁芤菰薩萨芭茎莖蓣蕷茅苔基莽蕻苊茇戎葫苦蓐萘葳薦荐葜茬藉藕菲匪若惹苈藶项項蕆蒇茂藏蕨其戡期斟綦勘甚斯蓁欺蘢茏菜艿菔莩藐蒯苴散蕤藤荽芨革韉鞯鞴韝邯靶鞣芸芰甘薷鞑韃芏葑鞋葑鞲蒜鞘靼薹勒蕾貳贰鞅葦苇鞍荸鞠芜蕪芫鞔莰鞫靳某著蓍鞒鞽薔蔷鞭菱芙靴藿七荊荆匡莆菁芋廿蔫苤蕙蔌芾颟顢巧戈弋莱萊葬薤茉蘋苹莢荚萋荑牙蘧芷苜芷苜菽苫颐赜賾頤熙臣臥卧邪鸦鴉迓雅茈東东茳蕖范範菹薄萍蒲菠莎藻荡蕩鶇鸫蒎菏落藩汞茫蒗划劃划蕞莫蓦驀墓幕暮募慕摹萌昔蒔莳草菖蔓葛鵲鹊藍蓝繭茧郾或葺蕺叵鄞堇勤惑觐覲匮匱蒉蕢颧顴莒鹳鸛萼莴萵功茵蒽蔑苗薯匣茴茄荔葸莲蓮瞢甍薨蘿萝鹋鶓菌匭匦蘇苏贡貢芟英苒茼匝萜檾苘莧苋黃黄芮鞏巩跫蛩恐銎世苠芑艺藝芑巨蔚蔬葭忒薜贳貰蕊芯荬蕒蓼苡藎荩蔞蒌藪薮芝菪塋茔蔻莞蒙苧苎萱莹瑩荥滎萤螢營营劳勞荤葷煢茕菅荧熒莺鶯菀蓥鎣荦犖榮荣蓉蓿萦縈區区芪苑葡茚蘚藓蓟薊瓯甌荀敬苟茗苟擎檠警菟毆殴苞菊歐欧鷗鸥萄葱蔥蔸芴获獲茆莸蕕荻艾匹萏芡芍蔦鳶鸢茑匠葩菝茱芹萆苛茜苯蘸葙蕈菥菘攻苌萇医醫蕎荞翳蓬莠芊荇莛董翹翘薰尧堯蓰莉蓖薇蕃藜蘅莪萎莓蘩燕蒴莘薏蘄蕲菩蔽蒂蒡蔣蒋茭茨薪蒺蒹藺蔺切艽菇薅茛荨蕁蕭萧茹苕萸芄共苍蒼蓊茯蔡薈荟茌苻葵荃莶薟蔹蘞莜蓧芥葆巷孽蘖薛恭薛茶荷荼荏莅蒞蓓芬巫觋覡苁蓯花蕉苓萑劐芩藥药葒荭芎葤荮莼蓴蘊蕴薌芗芘蓽荜蒈芳蔗蘺蓠茺苄蔼藹薺荠薺蓑蒿槁藁芒匾荒蘆芦葶蘑蘼蔟莨萃菸蓄蕹");
            wbList.Add("了陳陈子陬耶取陉陘聂聶顳颞聚鄹娶承隋隳随隨堕墮陌隴陇阴陰阻際际孺陡阱陸陆陸陆阮陵卫衛耳隔陋阢陕陝耻恥陂蚩陟騭骘阽孫孙函丞巹卺隙遜逊阳陽陧隉隅隰职職亟聩聵陨隕阵陣孟隈猛勐出祟聃聘糶粜也孑孔耿辽遼院耽聹聍陀隱隐孢陶聊陷孤阪隍隗陴耵降聒阡隆陲阼孜联聯隧障陪聪聰隘限隊队附墜坠險险阶階除聆陛防孓孩陔");
            wbList.Add("以戏戲颈頸騏骐弁剄刭劲勁逕迳驱驅骁驍巯巰鄧邓驟骤预預予馳驰豫矛骛騖鹬鹜鷸鶩瞀蟊蝥蟊矜鍪柔婺双雙又壘垒叠疊颡顙桑骖驂驿驛駘骀駘骀骏駿馭驭骚騷參参三叁毿毵畚骑騎馱驮馱驮能驵駔甬勇恿通熊对對聖圣怼懟驪骊騍骒台邰馴驯怠炱迨駛驶勸劝駟驷骡騾觀观骋騁馬马骣驏巴厶駝驼允驹駒駁驳騶驺欢歡鸡雞骝騮牟骠驃矣驕骄骆駱骢驄骈駢驥骥艰艱難难駙驸驗验逡皴驊骅騅骓駐驻叉蚤骧驤驢驴骗騙騸骟駭骇");
            wbList.Add("在左厮廝砑礴厝磺砸碟礞奁奩砹硗磽砌礤硭顧顾耷础礎厄友磉磣碜码碼雄大磊靨靥砺礪飆飙堿碱廈厦廈厌厭厴厣碩硕厲厉勵励魇魘套餍饜胡郁鬱硼髻髡髭鬟髯鬃鬢鬓鶘鹕髟髦鬏鬈髫鬣髹奪夺奔磕砝辰辱蜃唇硅奎矸奈夸誇刳匏瓠磚砖奢壓压三研硎砘古硬鹹咸廚厨感礓磧碛戌厂廠石犬砰硖硤威碡丰豐戛存破慧酆砉彗戚蹙砧艳豔夏契恝挈砂耢耮磲耔耙耨硝耕耘耩耒耥耖耦耜耬耧耗泵耪耠耱百厚非裴韭蜚辈輩悲翡斐厘碍礙砷奄鹌鵪碣右夼曆历夯奋奮砗硨厙厍面碘碳碉硐布而耐鸸鴯厕廁恧耍矶磯硯砚碸砜页頁矾礬成碾臧嘏尷尴蠆虿趸躉勱劢盛迈邁尥砀碭尤萬尤万尬戊灰盔磷達达镟碹碇碗牽牵砣克磔砥兢確确礫砾爽砍鸪矽鴣原斫愿願碑廳厅硒廂厢碴奇剞欹帮幫邦壽寿焘燾砟硌硇砭硪故磁劂厥磙滾磋砬辜鷯鹩碚碰磅碲肆廄厩碌春砼奉奏鹼硷磴仄泰蠢秦舂雁赝贋礁碓龙龍龔龚聋聾礱砻壟垄砩砒襲袭太礅硫丈态態碥戍礦矿碎");
            wbList.Add("有肛腻膩腖胨臘腊膜朦服郛孕孚乳肥胫脛胎盈戤須须肱胯腓醃腌腼靦腠胧朧肽朋貘脬貊月豺豳燹鹏鵬豹豸邈貌肜貉貔貂貅豚肝肢腳脚腳脚肚虢肼膨臌腩肟脖朊爰膚肤肘且肫縣县悬懸脯膊腈胚膈助肺豕雎胰胩鼐胪臚膛胱尕膽胆刖胂腽膃腥肿腫腭齶臊腡脶肋胭胛腮爵脅胁肌腆股胴觅覓肭甩脲腸肠膦脒爱愛受膿脓脘腙腚逐腕舜膑臏胺腔膣胸胝脆朐胞脎膁肷遙遥胍腺鷂鹞繇脾采彩膘腰膝用胀脹胜勝胙腹胳乃膠胶胼腾騰塍滕膳媵朕胖臆脫脱膀膪妥腿舀腱腴脸臉胗膾脍腧脞脂奚及腦脑脏髒脐臍膻肮肪脈脉胲臁腑腋臃");
            wbList.Add("地载載堪霰霸坩卉堰哉域戴贲賁贲賁堞栽截塔裁邗支卻却動动盍劫翅運运魂埃云去雲去城震垮埯垵霏壢坜靂雳需颥顬顸頇塬原壟垅圾堋霾埒寺坛壇圭土封填埴卦款圬雩恚堵牆墙二垤博埔圩干幹十士寸雨坏壞坯埂垣塥埡垭坪直越墟趣坡趄矗趕赶址趔赴趟亍坫盐鹽起赳壚垆赵趙趋趨趱趲走趑超真趁颠顛示霄霪霈未嫠墚求裘逑救進进戟朝埋韓韩埘塒坦堤遘覯觏刊坤亓井墁塌埸乾翰斡吉颉頡颉頡袁埕圳露酃埚堝塤埙喜鼓彭鼙瞽嘉甏熹协協雷塄南垌击擊垲塏垛献獻壩坝坍志堀韋韦霞韫韞违違韜韬韧韌墀霹馨磬罄謦圯圮圯圮聲声場场賣卖觌覿亏虧塊块坭專专赤郝赧赫赭赦螫過过勃孛鵓鹁垸壹彀觳彀觳懿毂轂壳殼壳悫愨壶壺索坨无無坻元頑顽堍黿鼋雹坞塢遠远均坎垢阪坂圻埤坼霜堙坷霖才孝教霆考埏者都翥煮雾霧垧坰圪老耋耆耄霉黴增丧喪幸垃境培嗇啬墒霎雪垠埭埽霓夫替輦辇逵規规埠堠齎赍麴堆零霍墊埝坶坳墳坟墩堖垴霁霽霭靄壤坑坊垓壕圹壙雯塘墉");
            wbList.Add("一開开邢琪形武琊鹉鵡弄型刑瑾瑛璜珙屯珥頓顿頓顿到至郅臻致臻瑪玛天瑚蠶蚕吞顼頊忝琦珑瓏表青瑗甫逋敷靓靚靛璦瑷静靜瑤瑶琢于於琺珐球盂瑋玮迂玩五琶王戔戋環环瑟琴琵玨珏下琥璩玻正焉鄢玷丐忑璨政甭歪邳丕否琐瑣還还珧孬理墼軎亙亘芈羋惠瑁丌更琨事吾囊蠹璐橐束整剌賴赖速敕卅噩副逼融鬲翮带帶吏豆豇豌豉逗畫画盏盞柬珈琏璉现現曹遭瑞再刺棘枣棗珊瓔璎璣玑责責丙邴兩两璀麗丽郦酈鹂鸝邐逦与與珉瑉琚歟欤瑕瓦来來嚴严亞亚戩戬堊垩晋晉恶惡恶璞滅灭赉賚琰琮珲琿琬琛列殛殆殂殍殖殪麸麩殘残歹裂虺烈殞殒殃殁歿殡殯殉殊殲歼聱骜驁獒鼇鳌螯赘贅熬遨鏊殤殇敖殚殫兀殓殮殄餮殄餮死玖珠碧珀瑰末玎珂琳玫珩瓚瓒珞璁麦麥平璋夾夹郟郏頰颊妻瑙珍玳瑜玢玲互纛毒素夷玉璃琉斑瓊琼班璇璿琅瑭玟");
            wbList.Add("上虎虐遽眶瞒瞞虞瞄虜虏彪虑慮虚虛覰觑眍瞘虔皮頗颇眙眸攴睃睚肯瞬睬睦歧瞌眭睹睛盹盱眄凸睐睞睫止目卡矍氍瞿眥眦蔔卜步叔督頻频顰颦瞠眺眇舊旧卓桌占战戰覘觇乩点點卤鹵卣鹺鹾贞貞盧卢眠瞰鼎顱颅鸬鸕矚瞩忐眯瞵瞎壑睿瞑眈餐粲瞻眵睁睜睥盯瞟睡瞅眨瞳睇眼瞍睨具齿齒龄齡龈齦龌齷齟龃齪龊齬龆龉齠齙龅齜齔龀龇齲龋瞪睽睑瞼盼瞧睢此砦些赀貲觜柴雌紫訾眩");
            wbList.Add("不江湛漭澌淇泔懣懑滿满漠灌潢彙汇泄渫渠涝澇瀠潆滢瀅漚沤鸿鴻柒浇澆沏潇瀟洪港池滠灄洱涵凼氹泐滁汉漢涇泾渗滲涌湧澤泽治灘滩浚汊尖湖洧涯溽沽沣灃灩滟尜淹瀝沥湎源濤涛潦溱泷瀧汰肖浮淝沮削逍淫滔溪汲涿法灞溘濡尘塵洼窪汗汁汗滇潮瀚潔洁澎澍汙污瀆渎渤沅渚潛潜汪沌添清溥浦沔洹滯滞浯涑瀨濑漱渍漬沛漕涞淶洌沫浅淺浹浃小滤濾滹波婆淚泪涉淑濒瀕渺淖沾湞浈瀘泸瀣濉水消淼淌洮沙裟鯊鲨挲娑浊濁瀑沓遝涅汨汩温溫漫溻濕湿渴濫滥混灝灏澡涓溃潰潞浞澠渑澠渑漶涡渦渐漸涸洇渭溷潿涠泗淠洄涟漣泅漯没沒澧湍泱滑油洞溅濺汕測测沿尚氅敞涔沁泯澉潺汛漏渥湄涮汜湯汤燙烫泌潑泼濯泥淡瀵学學黉黌浓濃嘗尝淙浣演泻渲瀉渲澱淀澩泶瀋渖溟黨党裳堂常赏賞掌棠渾浑沉覺觉覺涫鲎沈鱟滨濱泞濘嚳喾滓深溶沱光汹洶溝沟淆浠澹鼗淦漁渔濼泺洵浼渙涣泡耀逃辉輝淘泖潴瀦漪兆汐溜泊湃派湟浜沂洙汽浙激灑洒漂湮酒汀湘潭渣河淅淋潸濫漤溧漆淞沐沭少洚沲活鋈沃洗浩省溴洎濞涎潲洛劣洫澳汔淵渊潘湫泛滌涤潷滗海洋溯滚滾湔泮泣润潤瀾澜漾漳潼澗涧涪滴滂溢滋涕当當溉津潯浔汝溲淥渌沼洳淄浸染梁粱涩澀兴興沧滄脊举舉浍澮洽瀹渝潋瀲澄濮滏塗涂溆漵汾湓浴沦淪淮泠雀誉譽漲涨泓沸溺潍濰注渡淳漓流鎏澈汴濟济浏瀏沩潙澶沆沪滬泳漉瀛灣湾灤滦濠洲遊游浒滸漩濂浪溏液淬淤泫汶");
            wbList.Add("是虹蜞蟒蚶蟎螨蚜蜡蠟蟆蜮蠛蟥蝶蠓蝾蠑曉晓蛲蟯暴蠖最蜢緊紧肾腎蛹坚堅蛏蟶螞蚂贤賢蛑豎竖晨蛎蠣蝴蝰蛄蚌晟螈昃蝽螓明蜉暖蛆盟曖暧时時野墅曇昙裏里蠕蛙旰昧旱蟛蝻暑蚨量蛭昊晴蜻晡旦旺韙韪题題匙蝦虾蟪晤蝠師师螬蛱蛺早冒蠼申勖曰暢畅蟲虫晃蛸螳晁昌曝蛳螄晶日蠍蝎蜴蜾蝇蠅蜈蝸蜗曙曼蛊蠱蝈蟈蛔螺遇蛐映蚺蜩蚰帥帅愚禺蟣虮蚬蜆蚋电電暇虯虬遢曜昵顯显炅螻蝼暈晕暄螟暝晖暉蜿晏蛇晚昂蟾蟓煦冕易剔昴昀歇曷遏蝗蚯昕蛛蜱果颗顆螵曬晒蚵夥晰蜥杲昨蜂蛞蜘蠔蚝蜓星戥曛蚱晷螅蜒蜊蝮晌虼蟋蟠蛾蝌晦临臨监監鑒览覽鉴暗蛘蜷蟮蜣昱蚪曦蟬蝉蟑蛻蜕螃蛟蝤歸归旯螋昭照蛤蚣暌蝓蚧蜍晔曄暹蛉晗昆曳蚓蚴蚍景暾螭蛀影顥颢晾蛴蠐曩虻蝙蚁蟻曠旷蝣蠊螂螗蟀蚊");
            wbList.Add("中呀嘶喏嘞哐噶喵咂呓囈喋嘮唠哎嘔呕哓嘵哄嗒嗬嚆啊囁嗫叩嗤咄吼戢吧邑嗓啜嗵吗嗎吮哞唉唆叹歎顺順呃喹咕喊嘎嗄嘁啡喱嚦呖噅咴哒噠噘嘹唪嗪嚨咙吠呔吸咀嚼噯嗳鵑鹃啄叶葉喷噴吱嗑嚅哇吐嗔味嘲嘻嘭咭鄙喃哧噎嚏嗦呒嘸哮嘟嗜呋謔嚯呈吨噸郢吴吳哺籲吁籲嚇吓呸哽喇囔唔嗝嗽嘖啧嘈號号饕哑啞逞咧嗷唛嘜咦嗉距噱噓嘘躇唬蹺跷蹀蹣蹒躪躏蹑躡踊踴跆蹂跨跋躊躕蹰踌跖蹠蹶啃蹊踩蹈趿趺跬践踐趼贵貴遗遣遺遣趾跛踔咔哢跳踏踢蹋踝躁盅躅跏跺蹦跚踹忠踞跽蹼躞蹤踪蹿躥跎跑跪躒跞趵跌躓踬路跃躍踵鷺鹭踟蹯躜躦跹躚跣踽足蹉踯躑蹲蹄蹭踣跤跟躐踺趴啮跄齧蹌跗蹬嘴跸蹕呲卟蹁踱踉躔躋跻踮蹴吵哨啵嗩唢咣嗨雖虽嘬哩呻唱喁黽黾喝吕呂骂罵器囂嚣哭鄂鶚鹗顎颚串患品口噪鼍鼉咒另咽嗯喟嘿啭囀喂呷別别咖叻员員员員嗣郧鄖喘嘣啁吊勋勳嚶嘤叽嘰哕噦哚呐剮剐咼呙呗唄叫叼喔劈噼唰囑嘱呢唚吣噗啖嘍喽咪喧噥哝噻啶嘧咛嚀喀咤嚓史兄唏嚕噜喚唤咆嗚呜哆吻啕唿吹鳴鸣听聽唣唕呱嗥哌啪哳啤啦呆嘌哂叮喳啉呵啉噤噬呼噬唾吒唾咱嗅咋川咯響响噢吃啾哦咚啼嗍咩嗟喑噫噌嘀咬唼嗌哪呶唧哏嗖啸嘯叨只呛嗆嗡咐哙噲啥哈喻噔喉嗲咻吩咿唑嘩哗叭呤噙呤噙唯吟噍哟喲喙吲呦叱吡嗶哔咝噝喈吆嘛哼唷唁嚌哜嚷吭詠咏唳咳嚎嗾啷啐");
            wbList.Add("國国軾轼羁羈囝輒辄轻輕轟轰輟辍因轭軛固囿罟軲轱罪罨恩畸疇畴輳辏畎胃轩軒罢罷罴羆畦置轅辕罱转轉圍围黑黷黩默黪黲墨黥黯黠黝黜黔黟黢园園团團署圉車车囤輊轾畏圊圃輔辅罡罘畀辐輻圄輛辆四罩皿甲轤轳輥辊加辑輯駕驾回贺賀圆圓迦架哿軹轵袈男圜田勰嬲軸轴輞辋思輾辗轧軋罹轔辚边邊辖轄连連畹罗羅轹轢蜀邏逻囫軟软鴨鸭斩斬畈塹堑暫暂鏨錾椠槧軼轶困町轲軻力轎轿略辂輅圖图軤轷畋較较圈畔疃罾轨軌囡軺轺輜辎軔轫办辦轸軫畛輸输輇辁囚界轮輪囵圇囹累毗罚罰辙轍詈辘轆");
            wbList.Add("同曲贼賊嵌岈崬岽崂嶗嵘嶸岖嶇典郵邮凤鳳岜嶧峄贻貽殳峻央贿賄岩賑赈崖岸岵崴崦盎遄颛顓炭鴦鸯崎骨髓骷崩胄髁髑骰髏髅髂髖髋髕髌鶻鹘鶻鹘骶骺髀骼骱骸岌财財岐冉峙巅巔赇賕周雕贖赎赌賭賦赋岍賻赙嶼屿崃崍夙贱賤峡峽由帱幬幃帏幅幘帻帖帔幀帧帽幔幌巾贴貼幟帜帼幗帆幄貝贝幞襆帕帙帳帐幡幢幛帷峭嵴脊則则賜赐巋岿迥貺贶嶄崭崮岬崽冊册岫凹刪删峒兕山崗岗岘峴嵐岚罌罂嬰婴鹦鸚岂豈岷崛嵋剀剴凱凯覬觊屺赆贐賧赕嶙嵝嶁迪崇贮貯崆風风见見购購贍赡崤飓颶岡冈刚剛峋刿劌岣飈飚網网颮飑峁岁歲崢峥颼飕販贩凰嵬朵剁岢崾崧幾几账賬峰嶠峤賂赂屹貶贬峨嵊巍敗败赠贈嵯嶂赔賠赚賺赚嵫罔內内赊賒嵛崳嶝肉峪崔岭嶺岑嶷凡崞丹彤赃贓嵩賅赅");
            wbList.Add("民展惬愜異异羿惜懵屉屜慪怄懂愍湣殿臀慌敢孱懾慑屈屙憨怪慘惨懌怿怡悛居惰懨恹怙憾劇剧悱怖憂忧恢导導愤憤層层恸慟怯忮醜丑懦恃慎慰尉熨迅悖怃憮屠悻忖怀懷屋刁情怔恒司悟悚懒懶怦愫收眉戕奘爿胥遐疋蛋悼鹛鶥惧懼疏悄屑尿惝恍犀慢慳悭悍悝怛愠慍惕惺憬避悒忡愦憒愕辟臂襞壁甓鐾擘檗嬖劈譬璧譬慚惭翼悃愣届屆惴怏惆恫刷惻恻愷恺惘憶忆巽忸己巳尸屍书書局乙已忌乜快怩羽屡屢屎忱恽惲惋懈恂惚怕惶忻愾忾愧憷必怵必怅悵悸恬尾屐忏忤懺性怍屣虱愎属屬恪恤懊愀屨屦履悔改習习屏羼買买翌飞飛憚惮憧悅悦憎慊悌悯憫恨慨怊忉尻愉怆愴忪戮鹨鷚恰惟戳怜憐翟憔尼慣惯怫屁心恼惱惦忭尺惊驚昼晝咫懍懔忙遲迟尽盡慷慵悴翠悴");
            wbList.Add("爲为煤炬煉炼燒烧烘糞粪籽烃烴糅糝糁粑類类糲粝糊煳炻烦煩燎粗爝灶竈糯煒炜烤業业邺鄴燉炖精粳焐炳糟凿鑿凿鑿黻黹黼粘焯炒燭烛爆焊熳煜熾炽燥烟煙煨灿燦籼秈炯斷断糈熠煬炀炔烬燼炎郯剡火焱迷烷粽煊熔炮遴粼烯燃烁爍煥焕焰炊灼熘溜煌粕燈灯粞烽糙炸糗糌熄烙燠燔烀敉料烊燧烂爛粒糕焙熵焖燜糍娄婁退煺数數数數粉熗炝煆煅燴烩糇餱烨燁焓糨米炷炕炉爐煸煽燮糖糧粮糖糠粹炫");
            wbList.Add("这這这這宽寬宦寞字害宏寡宥宕宸割豁寄寮宠寵家宜农農冢塚守宗寒騫骞褰塞蹇赛賽搴寨謇完寇冠定室宇宣富寅寫写寶宝寂宵审審幂冪寓冥宴宮宫军軍郓鄆寰皸皲宙冗官甯寐寤逭密蜜宓寥灾災之宛剜冤鸩鴆宾賓牢宁寧宋客宅憲宪額额實实褡衩裰褥衫衬襯褂褚襦褚襦袜襪裱补補被襠裆宰袒襤褴褐裼裸裎裢褳袖衲袂裾褶褸褛袍裨褫襟襻袄襖衽袼袢褙裥襇初寢寝裉裙褪裕袱襝裣袷衿褓繈襁褲裤褊安宄案空竅窍穿竊窃窿突宿罕竇窦察窥窺窒窀窕窠窜竄窝窩窮穷帘簾窟窶窭穸窑窯窳窗窄窖窆穴窨邃究窘容窬穹窈鹤鶴它社祺祁禱祷祓禊祜祖祛禧福祆祉祯禎祧神祝禍祸視视祠祀禮礼禰祢禰祢祗祈祚祥禪禅禚祿禄禳");
            wbList.Add("我氏锘鍩鉗钳舛鋣铘桀错錯鏌镆昏鉕钷錨锚鉅钜钺鉞铽鋱铹鐒铙鐃镬鑊氐邸鸱鴟凶镊鑷铒鉺钌釕眢锰錳怨迎鴛鸳锕錒色鐸铎勾鈀钯钗釵然危郄郤钹鈸肴銪铕錛锛鈷钴锲鍥希郗钸鈽欷鋮铖鑄铸鐐镣鐝镢詹钛鈦角鋝锊锾鍰鑰钥鑰觫鋤锄觥觸触觖觚钐釤觴觞斛觶觯解蟹邂针針钍釷镇鎮鐳镭鍺锗铐銬銠铑錢钱鲽鰈鳓鰳印鲰鯫钝鈍鲐鮐鰠鳋魷鱿鲅鮁鯡鲱鲔鮪鮞鲕鱖鳜鋪铺錆锖鳐鰩匍鱼魚鲑鮭鳍鰭鲒鮚鲼鱝鲮鯪鳕鱈鮃鲆鯖鲭鯖鯁鲠鱺鲡鈣钙鉦钲鲇鮎鲈鱸盥鈈钚魯鲁鰻鳗鲣鲥鲤鯉鰹鰣鯧鲳鰨鳎鲲鯤匐鎘镉鱷鳄鰓鳃鲴鯝鳏鰥鰱鲢鲷鯛鱧鳢鲺鯴鳞鱗錸铼鲩鯇鲍鮑鰉鳇鰾鳔鯀鲧鰍鳅鲦鰷鰒鳆穌稣鮮鲜鱒鳟鳝鱔鲛鮫鋏铗鲟鱘鲫鯽鲻鯔鯢鲵鮒鲋鲶鯰鲸鯨鯿鳊鳙鱅鱭鲚魴鲂鈺钰外铍鈹钼鉬钻鑽钋釙乐樂乐銷销璽玺鎖锁鐋铴迩邇鏜镗銚铫鈔钞尔爾鐺铛鐺铛旬郇铿鏗象鋰锂鉭钽锝鍀钊釗刈镘鏝龜龟锡錫锞錁錕锟名句鋥锃钏釧钟鍾铝鋁锷鍔劬鍋锅銱铞免鴝鸲勉逸夠够兔甸錮锢銦铟鈿钿鉀钾锶鍶链鏈鑼锣鐲镯鏍镙負负奐奂铀鈾铜銅鍘铡铅鉛铠鎧鋼钢钠鈉钡鋇钒釩包饒饶馍饃馑饉馓饊餌饵飴饴锯鋸餒馁鈕钮餑饽烏乌鄔邬饢馕飩饨饯餞镅鎇蝕蚀饅馒刨餛馄馈饋饥饑釔钆钇釓锔飼鋦饲饧餳馔饌馆館飲饮馏餾饱飽饞馋餡馅饭飯餼饩馇餷饿餓飫饫饪飪飾饰饷餉飭饬鉍铋饼餅饈馐餃饺餿馊馀餘铌鈮炙鬯镤鏷錟锬鏤镂鈥钬錠锭鎵镓夤鑌镔銨铵鑹镩鑔镲铊鉈多匈鈎钩铯銫镥鑥铄鑠銘铭钨鎢鑫金鍃锪铩鎩铆鉚爻鈞钧錚铮欽钦鎦镏釣钓鐵铁鈑钣勿铂鉑銖铢刎锦錦忽兜鍁锨匆釘钉弑镖鏢缽钵刹镡鐔鈳钶殺杀兒儿猎獵狨貓猫獾犯卯猻狲猛狁犸獁猱狻鋒锋猢獠猗犹猶獗狙鏽锈钎釺銩铥猿犴豬猪鋯锆铤鋌铤铣銑铣櫫橥鍤锸狂猜锤锺錘鍾獺獭逛狹狭鎳镍獨独狸猩獅狮猖猓鉻铬狷狎蝟猥猬猡玀猾狈狽狃猸夕鍬锹逖狄獰狞狩狗獬猙狰麅狍狐鋨锇猹猁獯狡獐獍狠卿犰猊猴獪狯猃獫猞狳狒狴獼猕狼狱獄猝狺獷犷勻匀錈锩鈄钭鎂镁锎鐦鑭镧锌鋅镜鏡鐧锏鐿镱銳锐锫錇镝鏑鍆钔鐠镨镑鎊鉸铰鏘锵铲鏟镒鎰銻锑爭争鄒邹银銀刍芻鍵键釹钕锼鎪皱皺铷銣锱錙急锓鋟煞雛雏欠锻鍛铨銓鉿铪鎿镎镫鐙銼锉铧鏵锥錐铃鈴飧镌鐫鈐钤鐨镄鏹镪锴鍇久鍍镀鐓镦鐓銃铳锍鋶訇銥铱锿鎄勺镶鑲鎬镐鎬铈鈰鈧钪鈁钫鹱鸌嫋袅鸟鳥凫島鳧岛鑣镳枭梟鸵鴕灸卵孵镞鏃镟鏇鐮镰貿贸鋃锒鏞镛留遛铉鉉");
            wbList.Add("的找拭皂撕撒扛措摸揠描拒揲拣揀撈捞抠摳挠撓拱搭搽报報揶摄攝拯拙反拚抒揉掇搡掺摻捅择擇择抬擡把返挨摊攤瓜搔瓞拓扼拔振捱拜捺皋臯挎翺翱撼排掩颀頎擾扰撻挞掎撩撅捧揍攏拢扔援捋擁拥授搖摇持技挂盾遁擀拮撷擷擂質质抟摶挝撾抚撫拷扶撵攆攉後后邱卮丘掭捕搏皇攮捂逅嶽岳遑抹乒挟挾抨捷兵乓年掳擄摅攄卸披看扯攫掉牛拈撲扑爬爪朱邾捎撐撑攪搅挑抄泉挡擋提撮捍捏担擔抻揭揽攬拽扣揖挹捐捉操拐捌损損押摁擺摆舞擐掴摑捆撂摞抽缸罐投揣帛罅制掣缶攖撄缺皚皑摧所抿搌氣气掘撖氫氢据據氪氖扭握氰氬氩氘氲氳擗氚氤氙扎紮撰氮氨扬揚摺拨撥氕氡氧摒氯氛擢抉氟拟擬氦摟搂擻擞近迫皖揎揮挥摈擯擰拧按控攛撺擦挖探换換抵抑鬼魑魃魈魁魅魍魎魉擼撸拘挽搀攙抱掬撳揿掏掐挣掙欣搗捣折扳皈拍搋抓蜇哲踅逝掀魄白捭拆誓打摣揸手托拖括挢撟搬插郫鹎鵯扡扦卑皓攢攒攢攒撬挺捶擤攥揪皤播皤斤拉拼搠搓掷擲拦攔拌抖拌撞掸撣揞掊摘撇扪捫皎撙擱搁接搛掃扫挪搜招抛殷拶捃執执垫墊鷙鸷絷縶蟄蛰势勢贽贄热熱挚摯失搶抢拊拴揆撿捡揄拾搿迭叠扮掰挫抡掄扒拎擒携攜推捻撚批掾掼摜拇指拂拗搦揩擴扩摭撤拄掂斥掠擠挤攘擅搞抗护護捩掮邀敫搪掖摔搐");
            wbList.Add("要械栉櫛棋榧椹柑杠框棟栋模槿横橫櫃柜檬枢樞柩桡橈李椰椭橢權权杼橘懋桶樹树柽檉榪杩杷梭杈枯柘橱櫥楔槭櫪枥頂顶椅梆橛棒楱椿榛栊櫳杖極极桴楹棚杉村鄄枝桂樗杜甄杆植樾標标剽飘飄柰票瓢桔檑楠櫝椟材楮栲栳樯檣棱本醋酣醛酏酸桎酉醐酤醢酞酹醞酝酵酐酎枉酽釅酾釃酾釃丁醵西杯醒桓醍梗醌梧酲楝柄槽酮醴配醪醑醪朽醚椏桠醭酡酌酗酩杌酊棧栈酢酷酪酷醺酰酶酥枰醅酚醮酴酯醉醇釀酿酬醣醯相椒棹楨桢想栌櫨朴樸档檔梢桫樘桃桄杪榉櫸查杳覃榻棵檻槛檻欖榄棍可楫杏枵榀哥歌枳楞柙枷椤欏机機柚桐栅柵櫻樱榿桤枫楓枧梘賈贾枘楊杨橄椐栩楣榍樨杞札劄傑杰粟樓楼棕楦棺枕檳槟檳檸柠桉榨檫榕榷柁構构柢桅簷檐桷槲橹櫓櫟栎橡枸攀樊柳榴杓析板槔橰柏栀梔株棉槐皙檄柝林郴彬禁栖棲楂柯梵楚焚夢梦森木栗婪棼麓格枨棖桥橋栝橇杵桁杵梏梃棰柞榭楸覆梅枚样樣槎欄栏梓樟櫚榈榜校櫬榇樽梯概根楗桕棣欞棂检檢枪槍松椴桧檜栓榆橙槌枞樅樺桦椎檎柃檎榫樵楷椽櫞橼枇術术椁槨梳桩樁櫧槠柱椋榱檀檩檁杭柿槁枋核述榔");
            wbList.Add("和长長筇箕簸篚箬筐箧篋逄升笸築筑簧簪乇徹彻筮季迤麽么径徑笆篤笃笞私知矩甜榘舌短舔箴逢夭籍乔喬徘刮智岙嶴乱亂稽嵇适適鸹鴰舐敌敵籌筹矫矯憩矮矢辞辭矬雉笼籠矧秀舡艨稃艋租徂艚艫舻舟艄舯盤盘筋般磐舳舢船艦舰艉透舵舶舨徭舸舾艇舴艏艟稻艘艙舱舭航舫艤舣笈舷行丢丟壬竺街等待稈秆稹徒午竿篝千郜靠告鹄秸稭鹄鵠造德毛毪氌氆氇氈毡毳毯毽迁遷廷迕先贊赞選选筠箸乖穡穑歃生垂笄箐竽征眚惩懲重穗筻衙籟籁簌熏甥策箦簀徕徠箋笺秣秤秉处處算彼自篡纂臭乍怎迮片版牘牍牌牒牖徙衢昝咎鼻鼾齇齄劓鼽息延臬秒稍筲衍愆簿徜箔得香笪篩筛利犁梨稞复複篮籃馥各程種种篑簣稆穭禹積积雒务務血篾备備睾箅徊笳憊惫衄粤粵籩笾籮箩囱囪囟衅釁稷向秃禿役身射躺頹颓軀躯躲躬秧笛稠微筒徽徵徽徵幣币奧奥穢秽筧笕秘乞笥筷迄秋釋释番鄱翻釉悉籪簖愁簍篓管稼乏箢箜称稱稀衡衔銜徇笱移笏篼黎黧稳穩箏筝物箍箝筢籜箨犄牯特犢犊牡循徨篁牾禦御篪犋笊穆筘鵝鹅籀牺犧牧稗牲牿牦犛犍秩牝徼犏犒條条笨箱簟篥秫筆笔笑篷筅竹笙笮筵篦秭笫秭禾科徉箭笠乎簡简箪簞螽稅税冬乘剩委很簋律箫簫笤逶魏筍笋答筏符筌签簽簦筱黍黏篌徐稚稔第篆胤每鳘鰵繁毓敏系後秕筚篳入篱籬往汆穰稿篙彷篇簏籴糴簇稂");
            wbList.Add("产産並并疳瓶瘧疟瘧凍冻瘼閾阈癀迸癆痨瘩聞闻疖癤療疗瘾癮逆朔塑癃槊馮冯痙痉闯闖痛袞衮冶兗兖疤竣瘫癱瘙關关差郑鄭卷闳閎羟羥羧閆闫豢减減著着眷瘛頭头羊痱判閹羯阉羯癧疬羰羞羌翔疣癘疠送闥闼羝鲞鯗拳叛桊棬鄯善券湊凑羚状狀誊謄养養前疽翦煎剪痈癰毅遂阌閿瘃半闔阖蘭兰闺閨痔壯壮阗闐癲癫斗鬥闈闱痣闭閉凌淩装裝閏闰閂闩美靖症痞辣痦竦瘌癩癞痘蓋盖闌阑病恙羔羹痖瘂冽羲羨羡瘗瘞薑姜凄淒冱痍癍站疲阒闃癯臒丫夔疵冰痧阔闊瘠間间鄣彰音郸鄲囅冁童單单疸闽閩章閶阊瘟意竟竭韻韵歆赣贛戆戇戆戇韶部問问瓿冲沖剖癌闾閭总總况況竞競兑兌曾痼獸兽痹甑闸閘痂瘸閫阃瘰商疫端凋敝弊鼈鳖瞥蹩瞥蹩憋疝癭瘿瘋疯飒颯决決癜闞阚闞阚翊瘕悶闷癖瘍疡瘳普痰瘘瘻帝啻旁交閽阍郊獎奖将將癣癬浆漿鬮阄痪瘓疱皰桨槳醬酱效净淨閻阎次咨盗盜资資恣瓷粢姿疚瘤瓣亲親瘭尊猷奠酋遵遒遵遒閑闲疔疴屙鹇鷳新凇疾癡痴瘢顔颜彥彦冼首痄馘道癟瘪痢閣阁疙疼痿立闕阙痒癢瘥阐闡癉瘅瘴癔阅閱癇痫妝妆痕妾瘦兼鶼鹣歉鬩阋瘐闪閃阀閥疮瘡疹瘵痊阕闋疥益蠲瘊痤准冷北邶背弟鵜鹈剃遞递冀凝辮辫茲兹孳鶿鹚慈六疰辛门門闵閔凉涼凛凜闹鬧闶閌閡阂辨辯辩閬阆瘁閼瘀阏瘀痃");
            wbList.Add("發发毁毀媒姬舁嫫嫗妪娆嬈好媸她婀媽妈既暨即孥妤駑驽始努帑胬怒弩奴姑娠奶墾垦媛姐艮懇恳退嬡嫒尋寻尋那妓娃奸聿建妹媾嬉妩嫵孀姥嫱嬙姨妍婧婊嫣嫩娅婭婕叟錄录隸隶肅肃剥剝逮嫦逯姚妙旭旮娌妲娼媪媼巢剿如邵邕娟召娛娱劭媧娲恕迢絮舅姻甾邋嫘妯娉姍姗刀妞媚婿妃巛娓鼠鼷鼯鼬鼹鼢鼴妮姒靈灵巡嫁婶嬸帚婉嫔嬪姹婚娩舄妁鸠鳩姝婢杂雜嫖九妖娇嬌尹妊姓臼媳群郡君媲姊娥嫌姘婵嬋嫜嫡姣娴嫻嫉馗娣妇婦娜嫂女臾妗姆妣妨刃丸刃嬀妫嬗妒忍婷嬤嬷娘");
            wbList.Add("人代偌仝垡伢借偃黛貸贷岱僭儆傴伛牮伐僥侥僥供袋他戧戗仓倉仔佴創创公頌颂俑瓮甕翁侔俟俊傩儺僅仅估佐侑侉仨佰俳俺佑段優优倚做俦儔僚傣俸伏仗仍俘傭佣会會僨偾郐鄶劊刽伎儒侍佳仁仕仁仕什值祭畲俅佘俅舍舒佶僖舆輿璺爨偉伟传傳佬佞付全倒侄癸傅倩俞毹愈逾覦觎伍劍剑佥僉斂敛便合龠命鸽鴿盒颌凳頜凳頜翕使拿登歙龕龛僵俩倆債债儷俪佤兮儼俨例傲侠俠个個企佧倬侦偵候修倏悠攸俱仆偿償俏倘儻傥佻氽介俚但伸倡偶偈倮保俁俣促仲侶侣侃堡煲佃偎儇偎儇伽儡仂仙倜佩侗俜側侧仉催亿億倔倨伺假僻阜追侯伲似夥伙偻僂儂侬佇伫倌傧儐倥佗你低爷爺仰爸儋釜您像佝侈爹斧傯偬父伯件侏傀俾佚休体體仃何僳鵂鸺傈作伥倀敘叙仫侨僑任仵仟仵凭憑赁賃恁俐傷伤傻仡途俄餘余斜佟倭侮八們们倦佯伴位伞傘僮倍僧傍佼分颁頒健坌盆贫貧岔仇忿侵倪伊仞從从耸聳傖伧傖佾俎坐儈侩傺叢丛儉俭偷价價穀谷鵒鹆欲慫怂份众衆俗伦倫伶僬化侖仑倾傾華华佛貨货仳偕信禽令鄰邻領领翎瓴依雋隽儲储隼隹住僦儕侪售伪僞伉仿偏今衾含颔贪貪頷念焦鷦鹪劁停仪儀集食俯雠讎");
            wbList.Add("經经红紅绒絨紺绀緙缂练練绁絏繞绕弛弭絀绌弪弳紓纾缀綴繹绎绐紿弘顷頃绂紱毋绔絝縟缛缄緘弼绯緋緬缅绮綺綁绑繚缭级級绷繃缓緩組组蠡绶綬彖綏绥結结紜纭疆缜縝缬纈贯貫续續緯纬鸨鴇绪緒綾绫紂纣线線純纯缚縛纡紆綆绠繮缰績绩彝彜缙縉母彘引緲缈绰綽纱紗潁颍綃绡绱緔糸旨緹缇绅紳弗缦縵费費艴纜缆绲緄强強緝缉絹绢缋繢缱繾強犟绳繩繰缲織织细細缳繯幼緦缌轡辔縲缧綱纲綢绸缨纓纳納纪紀缗緡纽紐弓糾纠幻幺么缪继繆繼缕縷粥鬻綜综绽綻綰绾繽缤縮缩縮缩約约約约纸紙絕绝弥彌絢绚縐绉綿绵弧繳缴縹缥缃緗張张绛絳穎颖疑肄疑縫缝鄉乡繡绣纖纤絎绗纘缵缍綞綹绺络絡络絡匕紇纥紇纥縧绦终終飨饗弱缮繕綣绻绊絆弹彈缯繒缔締绞絞縑缣缢縊綈绨綠绿綠缫繅紹绍缁緇紉纫紈纨給给緞缎繪绘緶缏缑緱缒縋纷紛縱纵纶綸纶綸維维比畿緣缘毕畢丝絲鷥鸶毙斃绋紼幽毖皆纰紕紡纺縭缡統统纏缠縞缟编編纩纊弦纹紋");
            wbList.Add("主度試试誡诫諾诺谌諶謀谋讧訌誆诓訝讶謨谟謹谨廑席諜谍詎讵庹庶鹧鷓遮謳讴譖谮诬誣谎謊離离郭諏诹享邝邡鄺邡亨詘诎烹鶉鹑敦憝熟塾孰充棄弃序袤谲譎育诵誦譯译詒诒诶誒庆慶诂詁诔誄誹诽齑齏庵齋斋诚誠诙詼龐庞衣谖諼诅詛裔谣謠哀诼諑计計庋莊庄诗詩诖詿訐讦讲講诘詰诘谳讞讀读讀諱讳廡庑諸诸討讨斌請请证證语語谏諫廒评評讓让謔谑店卞訃讣就應应鹫鷲诮誚谠讜京刘劉廛齊齐剂劑谩謾謁谒课課裹训訓吝誤误衰衷譴谴襄諤谔瓤識识謂谓畝亩库庫禀稟颤顫颤顫謖谡高亢设設庙廟頏颃调調訕讪市敲譏讥諷讽讷訥記记邙戾扉户戶肩肓望讯訊诩詡詞词盲永昶鹿麝麈麟麂麋鏖麇麒啓启扈蠃赢贏羸嬴羸嬴蠃遍扁扃翩氓扇忘刻肇綮谧劾謐劾頦颏亥废廢戽亡妄廖謬谬雇訣诀雇房變变弈孪孿奕蠻蛮峦巒臠脔恋戀谈談迹謎谜鸞鸾銮鑾攣挛欒栾亦娈孌彎弯谊誼豪膏诨諢亮亭毫亳诧詫義义底詆诋詭诡譫谵廨询詢谗讒庖诳誑诌謅諂谄诤諍訴诉詬诟诛誅庳亵褻订訂床譚谭訶诃麻麽魔麽魔磨靡縻麾糜摩放旗施话話旖誘诱膂旅許许诰誥庭詵诜旆詐诈誕诞謝谢旃旋訖讫旎族旌旄诿諉誨诲州旒说說说說详詳庠譾谫讕谰斓斕谙諳謫谪谱譜謗谤谛諦谘諮谚諺谦謙廉諡谥良朗郎庸裒唐康詔诏庚庾賡赓谀諛認认讼訟诊診府腐诠詮谕諭褒庥夜於座膺卒谶讖鷹鹰论論訛讹誰谁誰谁谂諗譙谯率紊詣诣畜雍饔壅玄庀庇諧谐方廓諄谆广廣文諒谅廩廪访訪谝諞廬庐该該議议廊誶谇言");
            wbList.Add("");
        }

        #region 关于异常
        /// <summary>
        /// 获取异常中的所有描述
        /// </summary>
        public static string InnerMessage(this Exception ex)
        {
            var msg = ex.Message;
            while (ex.InnerException != null)
            {
                string innerMsg = ex.InnerException.Message;
                if (!string.IsNullOrEmpty(innerMsg) && ex.Message != innerMsg)
                {
                    msg = string.Format("{0}\r\n{1}", msg, innerMsg);
                }
                ex = ex.InnerException;
            }
            return msg;
        }

        #endregion

        #region 字符串转换
        #region 中文转拼音(取汉字、数字、字母，其它自动过滤)
        /// <summary>
        /// 在指定的字符串列表cnStr中检索符合拼音索引字符串
        /// </summary>
        /// <param name="str">汉字字符串</param>
        /// <param name="args">换格字符</param>
        /// <returns>相对应的汉语拼音首字母串</returns>
        public static string ToSpell(this string str, params string[] args)
        {
            return ToCode(ToSpell, str, args);
        }
        private static string ToCode(Func<string, Tuple<bool, string>> action, string str, params string[] args)
        {
            if (str.IsNullOrEmpty()) return string.Empty;
            string strTemp = null;
            bool last = true;
            for (int i = 0; i < str.Length; i++)
            {
                Tuple<bool, string> result = action(str.Substring(i, 1));
                if ((result.Item1 || last) && !IContains(result.Item2, args))
                {
                    if (args.Length > 0) { }

                    strTemp += result.Item2;
                }
                last = result.Item1 | IContains(result.Item2, args);
            }
            return strTemp.ToUpper();
        }
        /// <summary>
        /// 判断非汉字时取值问题
        /// 参数为空时仅取连续字母或数字的首位
        /// </summary>
        private static bool IContains(string str, params string[] args)
        {
            if (args.Length > 0) return args.Contains(str);
            int c = str[0];
            if (c >= 65 && c <= 90) return false;//A-Z
            if (c >= 97 && c <= 122) return false;//a-z
            if (c >= 48 && c <= 57) return false;//0-9
            return true;
        }
        /// <summary>
        /// 根据一个汉字获得其首拼音字符
        /// </summary>
        private static Tuple<bool, string> ToSpell(string chart)
        {
            string result;
            //获得其ASSIC码
            byte[] arrCN = Encoding.Default.GetBytes(chart);
            if (arrCN.Length > 1)
            {
                //int code = (area << 8) + pos;
                int code = arrCN[0] * 256 + arrCN[1];
                //0~65535
                //a~Z的数字表示
                int[] areacode = {45217, 45253, 45761, 46318, 46826, 47010, 47297,
                                    47614, 48119, 48119, 49062, 49324, 49896, 50371,
                                    50614, 50622, 50906, 51387, 51446, 52218,
                                    52698, 52698, 52698, 52980, 53689, 54481 };
                //判断其值在那2个数字之间
                for (int i = 0; i < areacode.Length; i++)
                {
                    //最后一个汉字的值
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        //转为字母返回
                        return new Tuple<bool, string>(true, (char)(65 + i) + "");
                    }
                }
                result = "?";
            }
            else result = chart;
            return new Tuple<bool, string>(false, result);
        }

        #endregion

        #region 中文转五笔(取汉字、数字、字母，其它自动过滤)
        /// <summary>
        /// 在指定的字符串列表cnStr中检索符合五笔索引字符串
        /// </summary>
        /// <param name="str">汉字字符串</param>
        /// <param name="args">换格字符</param>
        /// <returns>相对应的汉语五笔首字母串</returns>
        public static string ToWbCode(this string str, params string[] args)
        {
            return ToCode(ToWbCode, str, args);
        }
        /// <summary>
        /// 根据一个汉字获得其首个五笔字符
        /// </summary>
        private static Tuple<bool, string> ToWbCode(string chart)
        {
            string result;
            //获得其ASSIC码
            byte[] arrCN = Encoding.Default.GetBytes(chart);
            if (arrCN.Length > 1)
            {
                for (int i = 0; i < wbList.Count; i++)
                {
                    if (wbList[i].Contains(chart))
                    {
                        result = Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                        return new Tuple<bool, string>(true, result);
                    }
                }
                result = "?";
            }
            else result = chart;
            return new Tuple<bool, string>(false, result);
        }

        #endregion

        #region 全角半角转换
        /// <summary> 转半角的函数(DBC case) </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        /// 全角空格为12288，半角空格为32
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToHalfAngle(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] == 12290)
                {
                    c[i] = (char)46;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375) c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        #endregion

        #endregion

        #region 数据转换
        /// <summary>
        /// Int32转换
        /// </summary>
        public static int ToInt(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;
            if (obj.GetType() == typeof(int)) return (int)obj;

            var value = obj.ToString();
            if (value.Equals("true", StringComparison.OrdinalIgnoreCase)) return 1;
            if (value.Equals("false", StringComparison.OrdinalIgnoreCase)) return 0;
            if (int.TryParse(value, out int data)) return data;

            if (double.TryParse(value, out double result))
            {
                result = Math.Round(result, MidpointRounding.AwayFromZero);
                return (int)result;
            }
            return 0;
        }
        /// <summary>
        /// Bool转换
        /// </summary>
        public static bool ToBool(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return false;

            if (obj is int iObj) return iObj != 0;
            if (bool.TryParse(obj.ToString(), out bool result)) return result;
            return false;
        }
        /// <summary>
        /// Long转换
        /// </summary>
        public static long ToLong(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;
            if (obj.GetType() == typeof(long)) return (long)obj;

            if (long.TryParse(obj.ToString(), out long value)) return value;
            return 0;
        }
        /// <summary>
        /// Double转换
        /// </summary>
        public static double ToDouble(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;

            if (double.TryParse(obj.ToString(), out double value)) return value;
            return 0;
        }
        /// <summary>
        /// float
        /// </summary>
        public static float ToFloat(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;

            if (float.TryParse(obj.ToString(), out float value)) return value;
            return 0;
        }
        /// <summary>
        /// decimal
        /// </summary>
        public static decimal ToDecimal(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;

            if (decimal.TryParse(obj.ToString(), out decimal value)) return value;
            return 0;
        }
        /// <summary>
        /// String转换
        /// </summary>
        public static string ToStrs(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return string.Empty;

            return obj.ToString().Trim();
        }
        /// <summary>
        /// DateTime转换
        /// </summary>
        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.ToString() == string.Empty) return DateTime.MinValue;

            if (DateTime.TryParse(obj.ToString(), out DateTime result)) return result;
            return DateTime.MinValue;
        }
        /// <summary>
        /// 检测obj,如果为DBNUll或空字符串 返回true
        /// </summary>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return true;

            return string.IsNullOrEmpty(obj.ToString());
        }
        /// <summary>
        /// 消除算术计算误差(double转decimal)
        /// </summary>
        public static double ClearError(this double value)
        {
            try
            {
                return decimal.ToDouble(value.ToDecimal());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return value;
            }
        }

        #endregion

        #region IList 与　DataTable 互转
        /// <summary>
        /// 获取接口所有属性
        /// PropertyInfo.SetValue不能设置只读接口
        /// </summary>
        public static List<PropertyInfo> Properties(this Type type)
        {
            var list = new List<Type> { type };
            if (type.IsInterface)
            {
                list.AddRange(type.GetInterfaces());
            }
            var pList = new List<PropertyInfo>();
            for (int i = 0; i < list.Count; i++)
            {
                pList.AddRange(list[i].GetProperties());
            }
            return pList;
        }
        /// <summary>
        /// 获取接口属性（可读、基础类型非空的泛型、值类型（含string、byte[]、Image、Bitmap））
        /// </summary>
        public static List<PropertyInfo> PropertiesValue(this Type type)
        {
            var properties = type.Properties();
            var vList = new List<PropertyInfo>();
            foreach (var property in properties)
            {
                if (!property.CanRead) continue;
                Type dbType = property.PropertyType;
                if (dbType.IsGenericType)
                {
                    if (Nullable.GetUnderlyingType(dbType) == null) continue;
                    dbType = Nullable.GetUnderlyingType(dbType);
                }
                if (dbType.IsClass && dbType != typeof(string) && dbType != typeof(byte[]) && dbType != typeof(Image) && dbType != typeof(Bitmap)) continue;
                vList.Add(property);
            }
            return vList;
        }
        /// <summary>
        /// 获取指定名称属性
        /// </summary>
        public static PropertyInfo Property(this Type type, string name)
        {
            var property = type.GetProperty(name);
            if (property == null)
            {
                var properties = type.Properties();
                property = properties.Find(c => c.Name == name);
                if (property == null) property = properties.Find(c => c.Column() == name);
            }
            return property;
        }
        /// <summary>
        /// 获取接口所有属性
        /// </summary>
        public static List<PropertyDescriptor> Descriptors(this Type type)
        {
            var list = new List<Type> { type };
            if (type.IsInterface)
            {
                list.AddRange(type.GetInterfaces());
            }
            var pList = new List<PropertyDescriptor>();
            for (int i = 0; i < list.Count; i++)
            {
                foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(list[i]))
                    pList.Add(item);
            }
            return pList;
        }

        /// <summary>
        /// 将指定类型转为DataTable
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sql">从数据取值时，Image存储格式为byte[]</param>
        /// <returns></returns>
        public static DataTable CreateTable(this Type type, bool sql = false)
        {
            var table = new DataTable(type.Name);
            foreach (var property in type.PropertiesValue())
            {
                Type dbType = property.PropertyType;
                if (dbType.IsGenericType) dbType = Nullable.GetUnderlyingType(dbType);
                if (sql && (dbType == typeof(Image) || dbType == typeof(Bitmap))) dbType = typeof(byte[]);
                table.Columns.Add(property.Column(), dbType);
            }
            return table;
        }
        /// <summary>
        /// 将指定类型转为DataTable
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateExcelTable(this Type type)
        {
            var table = new DataTable(type.Name);
            foreach (var property in type.PropertiesValue())
            {
                if (!property.IExcel()) continue;
                Type dbType = property.PropertyType;
                if (dbType.IsGenericType) dbType = Nullable.GetUnderlyingType(dbType);
                table.Columns.Add(property.Name, dbType);
            }
            return table;
        }

        /// <summary>
        /// 赋值（string数据转指定类型）
        /// </summary>
        public static void SetValue<T>(this T obj, PropertyDescriptor pro, object value)
        {
            if (value == null || value == DBNull.Value)
            {
                pro.SetValue(obj, null);
                return;
            }
            var type = pro.PropertyType;
            if (type.IsGenericType && Nullable.GetUnderlyingType(type) != null) type = Nullable.GetUnderlyingType(type);
            if (type.IsEnum) type = type.GetEnumUnderlyingType();
            switch (type.Name)
            {
                case nameof(Image):
                    if (value is byte[] buffer)
                    {
                        pro.SetValue(obj, StructHelper.BytesToImage(buffer));
                    }
                    else if (value is Image image)
                    {
                        pro.SetValue(obj, image);
                    }
                    break;
                case nameof(Int64):
                    pro.SetValue(obj, value.ToLong());
                    break;
                case nameof(Int32):
                    pro.SetValue(obj, value.ToInt());
                    break;
                case nameof(Int16):
                    pro.SetValue(obj, (short)value.ToInt());
                    break;
                case nameof(Byte):
                    pro.SetValue(obj, (byte)value.ToInt());
                    break;
                case nameof(Boolean):
                    pro.SetValue(obj, value.ToBool());
                    break;
                case nameof(Double):
                    pro.SetValue(obj, value.ToDouble());
                    break;
                case nameof(Single):
                    pro.SetValue(obj, value.ToFloat());
                    break;
                case nameof(Decimal):
                    pro.SetValue(obj, value.ToDecimal());
                    break;
                case nameof(DateTime):
                    var time = value.ToDateTime();
                    if (TConfig.IUtcTime) time = time.ToLocalTime();
                    pro.SetValue(obj, time);
                    break;
                case nameof(String):
                    pro.SetValue(obj, value.ToStrs());
                    break;
                default:
                    pro.SetValue(obj, value);
                    break;
            }
        }

        #endregion

        #region 特性
        /// <summary>
        /// 获取表名
        /// </summary>
        public static string TableName(this Type type)
        {
            return type.Table().Table;
        }
        /// <summary>
        /// 获取主键
        /// </summary>
        public static string TableKey(this Type type)
        {
            var attr = type.Table();
            return attr.Keys;
        }

        /// <summary>
        /// 返回特性，检查表名及主键或主列
        /// </summary>
        public static TableAttribute Table(this Type type)
        {
            var attrList = type.GetCustomAttributes(typeof(TableAttribute), false) as TableAttribute[];
            if (attrList.Length != 1) throw new ArgumentException(string.Format("类型 {0} 特性错误", type));
            if (attrList[0].Table == null) throw new ArgumentException("没有指定表名称");
            return attrList[0];
        }
        /// <summary>
        /// 获取描述
        /// </summary>
        public static string Description(this MemberInfo type)
        {
            var list = type.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (list.Length > 0)
                return ((DescriptionAttribute)list[0]).Description;
            return null;
        }
        /// <summary>
        /// 生成数据列标记
        /// </summary>
        public static bool ISelect(this PropertyInfo pro, out string column)
        {
            column = pro.Name;
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (list.Length == 1 && list[0].Column != null)
            {
                column = list[0].Column;
            }
            return list.Length == 0 || list[0].ISelect;
        }
        /// <summary>
        /// 显示列标记
        /// </summary>
        public static bool IShow(this MemberInfo pro, out string text)
        {
            text = pro.Name;
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (list.Length == 1 && list[0].Text != null)
            {
                text = list[0].Text;
            }
            return list.Length == 0 || list[0].IShow;
        }
        /// <summary>
        /// 获取列名
        /// 兼容视图多表查询、自定义列名
        /// </summary>
        public static string Column(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (list.Length == 1 && list[0].Column != null)
            {
                var column = list[0].Column;
                if (column.Contains(" "))
                {
                    column = column.Substring(column.LastIndexOf(" ") + 1);
                }
                else if (column.Contains("."))
                {
                    column = column.Substring(column.LastIndexOf(".") + 1);
                }
                return column;
            }
            return pro.Name;
        }
        /// <summary>
        /// 显示属性
        /// </summary>
        public static bool IBrowsable(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(BrowsableAttribute), false) as BrowsableAttribute[];
            return list.Length == 0 || list[0].Browsable;
        }
        /// <summary>
        /// 生成到ExcelTable标记
        /// </summary>
        public static bool IExcel(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(NoExcelAttribute), false) as NoExcelAttribute[];
            return list.Length == 0;
        }
        /// <summary>
        /// 自定义不复制列标记
        /// </summary>
        public static bool IClone(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(NoCloneAttribute), false) as NoCloneAttribute[];
            return list.Length == 0;
        }

        #endregion

        #region Equals
        /// <summary>
        /// 判断值是否相等
        /// </summary>
        public static bool TEquals<T>(this T t, T temp, bool child = false)
        {
            var type = typeof(T);
            return type.TEquals(t, temp, child);
        }
        /// <summary>
        /// 判断值是否相等
        /// </summary>
        private static bool TEquals(this Type parent, object t, object temp, bool child)
        {
            var properties = parent.Properties();
            foreach (var property in properties)
            {
                if (!property.IClone()) continue;

                var value = parent.GetValue(t, property.Name);
                var tempValue = parent.GetValue(temp, property.Name);
                if (!value.Equals(tempValue)) return false;
                if (!child) continue;
                if (value is IList)
                {
                    var tlist = tempValue as IList;
                    var list = value as IList;
                    if (tlist.Count != list.Count) return false;
                    for (var i = 0; i < list.Count; i++)
                    {
                        if (!list[i].TEquals(tlist[i], child)) return false;
                    }
                }
                else if (value != null && !value.GetType().IsValueType && value.GetType() != typeof(string))
                {
                    var type = value.GetType();
                    if (!type.TEquals(value, tempValue, child)) return false;
                }
            }
            return true;
        }

        #endregion

        #region 泛型
        /// <summary>
        /// 返回泛型实参数类型
        /// </summary>
        public static Type GenericType(this IList obj)
        {
            return obj.GetType().GenericType();
        }
        /// <summary>
        /// 返回泛型实参数类型
        /// </summary>
        public static Type GenericType(this Type type)
        {
            var types = type.GetGenericArguments();
            if (types.Length == 1) return types[0];
            return null;
        }
        /// <summary>
        /// 返回泛型实参实例
        /// </summary>
        public static IList GenericList(this Type type)
        {
            var listType = typeof(List<>);
            listType = listType.MakeGenericType(type);
            return Activator.CreateInstance(listType) as IList;
        }

        #endregion

        #region 含数字的字符串比较数字
        /// <summary>
        /// 含数字的字符串比较数字
        /// </summary>
        public static int TCompare(this object obj1, object obj2)
        {
            string x = obj1 as string;
            string y = obj2 as string;
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            char[] arr1 = x.ToCharArray();
            char[] arr2 = y.ToCharArray();
            int i = 0, j = 0;
            while (i < arr1.Length && j < arr2.Length)
            {
                if (char.IsDigit(arr1[i]) && char.IsDigit(arr2[j]))
                {
                    string s1 = string.Empty, s2 = string.Empty;
                    while (i < arr1.Length && char.IsDigit(arr1[i]))
                    {
                        s1 += arr1[i];
                        i++;
                    }
                    while (j < arr2.Length && char.IsDigit(arr2[j]))
                    {
                        s2 += arr2[j];
                        j++;
                    }
                    if (long.Parse(s1) > long.Parse(s2))
                    {
                        return 1;
                    }
                    if (long.Parse(s1) < long.Parse(s2))
                    {
                        return -1;
                    }
                }
                else
                {
                    if (arr1[i] > arr2[j])
                    {
                        return 1;
                    }
                    if (arr1[i] < arr2[j])
                    {
                        return -1;
                    }
                    i++;
                    j++;
                }
            }
            if (arr1.Length == arr2.Length)
            {
                return 0;
            }
            else
            {
                return arr1.Length > arr2.Length ? 1 : -1;
            }
        }

        #endregion
    }
}