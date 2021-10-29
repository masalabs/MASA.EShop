namespace MASA.EShop.Services.Catalog.Infrastructure;

public class CatalogContextSeed
{
    public async Task SeedAsync(CatalogDbContext context,
        IWebHostEnvironment env,
        IOptions<CatalogSettings> settings,
        ILogger<CatalogContextSeed> logger)
    {
        var useCustomizationData = settings.Value.UseCustomizationData;
        var contentRootPath = env.ContentRootPath;
        var picturePath = env.WebRootPath;

        if (!context.CatalogBrands.Any())
        {
            await context.CatalogBrands.AddRangeAsync(useCustomizationData
                ? GetCatalogBrandsFromFile(contentRootPath, logger)
                : GetPreconfiguredCatalogBrands());

            await context.SaveChangesAsync();
        }

        if (!context.CatalogTypes.Any())
        {
            await context.CatalogTypes.AddRangeAsync(useCustomizationData
                ? GetCatalogTypesFromFile(contentRootPath, logger)
                : GetPreconfiguredCatalogTypes());

            await context.SaveChangesAsync();
        }

        if (!context.CatalogItems.Any())
        {
            await context.CatalogItems.AddRangeAsync(useCustomizationData
                ? GetCatalogItemsFromFile(contentRootPath, context, logger)
                : GetPreconfiguredItems());

            await context.SaveChangesAsync();

            GetCatalogItemPictures(contentRootPath, picturePath);
        }
    }

    private IEnumerable<CatalogBrand> GetCatalogBrandsFromFile(string contentRootPath,
        ILogger<CatalogContextSeed> logger)
    {
        string csvFileCatalogBrands = Path.Combine(contentRootPath, "Setup", "CatalogBrands.csv");

        if (!File.Exists(csvFileCatalogBrands))
        {
            return GetPreconfiguredCatalogBrands();
        }

        string[] csvheaders;
        try
        {
            string[] requiredHeaders = { "catalogbrand" };
            csvheaders = GetHeaders(csvFileCatalogBrands, requiredHeaders);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
            return GetPreconfiguredCatalogBrands();
        }

        return File.ReadAllLines(csvFileCatalogBrands)
            .Skip(1) // skip header row
            .SelectTry(x => CreateCatalogBrand(x))
            .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
            .Where(x => x != null);
    }

    private CatalogBrand CreateCatalogBrand(string brand)
    {
        brand = brand.Trim('"').Trim();

        if (string.IsNullOrEmpty(brand))
        {
            throw new Exception("catalog Brand Name is empty");
        }

        return new CatalogBrand
        {
            Brand = brand,
        };
    }

    private IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
    {
        return new List<CatalogBrand>()
        {
            new CatalogBrand() { Brand = "Lonsid"}
        };
    }

    private IEnumerable<CatalogType> GetCatalogTypesFromFile(string contentRootPath, ILogger<CatalogContextSeed> logger)
    {
        string csvFileCatalogTypes = Path.Combine(contentRootPath, "Setup", "CatalogTypes.csv");

        if (!File.Exists(csvFileCatalogTypes))
        {
            return GetPreconfiguredCatalogTypes();
        }

        string[] csvheaders;
        try
        {
            string[] requiredHeaders = { "catalogtype" };
            csvheaders = GetHeaders(csvFileCatalogTypes, requiredHeaders);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
            return GetPreconfiguredCatalogTypes();
        }

        return File.ReadAllLines(csvFileCatalogTypes)
            .Skip(1) // skip header row
            .SelectTry(x => CreateCatalogType(x))
            .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
            .Where(x => x != null);
    }

    private CatalogType CreateCatalogType(string type)
    {
        type = type.Trim('"').Trim();

        if (string.IsNullOrEmpty(type))
        {
            throw new Exception("catalog Type Name is empty");
        }

        return new CatalogType
        {
            Type = type,
        };
    }

    private IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
    {
        return new List<CatalogType>()
            {
                new CatalogType() { Type = "中央处理设备"},
                new CatalogType() { Type = "纯水机" },
                new CatalogType() { Type = "饮水机" },
                new CatalogType() { Type = "胶囊机" },
                new CatalogType() { Type = "软水小产品" },
                new CatalogType() { Type = "商务机" },
                new CatalogType() { Type = "空净系列" }
            };
    }

    private IEnumerable<CatalogItem> GetCatalogItemsFromFile(string contentRootPath, CatalogDbContext context, ILogger<CatalogContextSeed> logger)
    {
        string csvFileCatalogItems = Path.Combine(contentRootPath, "Setup", "CatalogItems.csv");

        if (!File.Exists(csvFileCatalogItems))
        {
            return GetPreconfiguredItems();
        }

        string[] csvheaders;
        try
        {
            string[] requiredHeaders = { "catalogtypename", "catalogbrandname", "description", "name", "price", "picturefilename" };
            string[] optionalheaders = { "availablestock", "restockthreshold", "maxstockthreshold", "onreorder" };
            csvheaders = GetHeaders(csvFileCatalogItems, requiredHeaders, optionalheaders);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
            return GetPreconfiguredItems();
        }

        var catalogTypeIdLookup = context.CatalogTypes.ToDictionary(ct => ct.Type, ct => ct.Id);
        var catalogBrandIdLookup = context.CatalogBrands.ToDictionary(ct => ct.Brand, ct => ct.Id);

        return File.ReadAllLines(csvFileCatalogItems)
            .Skip(1) // skip header row
            .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
            .SelectTry(column => CreateCatalogItem(column, csvheaders, catalogTypeIdLookup, catalogBrandIdLookup))
            .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
            .Where(x => x != null);
    }

    private CatalogItem CreateCatalogItem(string[] column, string[] headers, Dictionary<string, int> catalogTypeIdLookup, Dictionary<string, int> catalogBrandIdLookup)
    {
        if (column.Count() != headers.Count())
        {
            throw new Exception($"column count '{column.Count()}' not the same as headers count'{headers.Count()}'");
        }

        string catalogTypeName = column[Array.IndexOf(headers, "catalogtypename")].Trim('"').Trim();
        if (!catalogTypeIdLookup.ContainsKey(catalogTypeName))
        {
            throw new Exception($"type={catalogTypeName} does not exist in catalogTypes");
        }

        string catalogBrandName = column[Array.IndexOf(headers, "catalogbrandname")].Trim('"').Trim();
        if (!catalogBrandIdLookup.ContainsKey(catalogBrandName))
        {
            throw new Exception($"type={catalogTypeName} does not exist in catalogTypes");
        }

        string priceString = column[Array.IndexOf(headers, "price")].Trim('"').Trim();
        if (!decimal.TryParse(priceString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal price))
        {
            throw new Exception($"price={priceString}is not a valid decimal number");
        }

        var catalogItem = new CatalogItem()
        {
            CatalogTypeId = catalogTypeIdLookup[catalogTypeName],
            CatalogBrandId = catalogBrandIdLookup[catalogBrandName],
            Description = column[Array.IndexOf(headers, "description")].Trim('"').Trim(),
            Name = column[Array.IndexOf(headers, "name")].Trim('"').Trim(),
            Price = price,
            PictureFileName = column[Array.IndexOf(headers, "picturefilename")].Trim('"').Trim(),
        };

        int availableStockIndex = Array.IndexOf(headers, "availablestock");
        if (availableStockIndex != -1)
        {
            string availableStockString = column[availableStockIndex].Trim('"').Trim();
            if (!string.IsNullOrEmpty(availableStockString))
            {
                if (int.TryParse(availableStockString, out int availableStock))
                {
                    catalogItem.AvailableStock = availableStock;
                }
                else
                {
                    throw new Exception($"availableStock={availableStockString} is not a valid integer");
                }
            }
        }

        int restockThresholdIndex = Array.IndexOf(headers, "restockthreshold");
        if (restockThresholdIndex != -1)
        {
            string restockThresholdString = column[restockThresholdIndex].Trim('"').Trim();
            if (!string.IsNullOrEmpty(restockThresholdString))
            {
                if (int.TryParse(restockThresholdString, out int restockThreshold))
                {
                    catalogItem.RestockThreshold = restockThreshold;
                }
                else
                {
                    throw new Exception($"restockThreshold={restockThreshold} is not a valid integer");
                }
            }
        }

        int maxStockThresholdIndex = Array.IndexOf(headers, "maxstockthreshold");
        if (maxStockThresholdIndex != -1)
        {
            string maxStockThresholdString = column[maxStockThresholdIndex].Trim('"').Trim();
            if (!string.IsNullOrEmpty(maxStockThresholdString))
            {
                if (int.TryParse(maxStockThresholdString, out int maxStockThreshold))
                {
                    catalogItem.MaxStockThreshold = maxStockThreshold;
                }
                else
                {
                    throw new Exception($"maxStockThreshold={maxStockThreshold} is not a valid integer");
                }
            }
        }

        int onReorderIndex = Array.IndexOf(headers, "onreorder");
        if (onReorderIndex != -1)
        {
            string onReorderString = column[onReorderIndex].Trim('"').Trim();
            if (!string.IsNullOrEmpty(onReorderString))
            {
                if (bool.TryParse(onReorderString, out bool onReorder))
                {
                    catalogItem.OnReorder = onReorder;
                }
                else
                {
                    throw new Exception($"onReorder={onReorderString} is not a valid boolean");
                }
            }
        }

        return catalogItem;
    }

    private IEnumerable<CatalogItem> GetPreconfiguredItems()
    {
        return new List<CatalogItem>()
        {
            new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 1, AvailableStock = 100, Description = "食品卫生安全材料 核心部件安全保障智慧流量延滞型再生模式 比同类产品省33%再生剂和65%的水 智能自动循环运行", Name = "R2中央软水机", Price = 9999, PictureFileName = "1.jpg" },
            new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 1, AvailableStock = 100, Description = "环保无铅 国标62铜材质 食品级高分子防爆瓶体 滤瓶刮洗 免拆滤芯 万向接头 安装不受限 可拆卸透明窗体", Name = "Q3全自动智能冲洗前置过滤器", Price= 9999, PictureFileName = "2.jpg" },
            new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 1, AvailableStock = 100, Description = "高性能高效去除水中余氯核心部件安全保障智慧流量延滞型再生模式比同类产品省33%再生剂和65%的水智能自动循环运行", Name = "J2中央净水机", Price =9999, PictureFileName = "3.jpg" },
            new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 1, AvailableStock = 100, Description = "大流量匹配大用水量食品级高容量离子交换树脂省盐省水大集成智能控制系统旁通阀设计", Name = "R3中央软水机", Price = 9999, PictureFileName = "4.jpg" },
            new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 1, AvailableStock = 100, Description = "2T/h大流量 三级滤料 高效过滤 自动记忆设定参数 方便省心 LCD显示屏 一目了然 旁通阀设计 维修保养便利", Name = "J3中央净水机", Price = 9999, PictureFileName = "5.jpg" },
            new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 1, AvailableStock = 100, Description = "大流量 3.5吨/时 优质树脂 有效软化水质 自动再生 便捷省心 延滞型再生 避免无水可用", Name = "朗诗德R4中央软水机", Price = 9999, PictureFileName = "6.jpg" },
            new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 1, AvailableStock = 100, Description = "大流量  3.5吨/时 三级滤料，高效过滤 正反自动冲洗  解放双手 延滞型冲洗  避免无水可用 断电记忆  方便省心", Name = "朗诗德J4中央净水机", Price = 9999, PictureFileName = "7.jpg" },

            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 1, AvailableStock = 100, Description = "3合1一体式专利复合滤芯 智能检测  安全防漏 超静音制水系统", Name = "云湃智能物联纯水机", Price = 9999, PictureFileName = "8.jpg" },
            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 1, AvailableStock = 100, Description = "进口陶氏RO膜 去除率达90%以上 五层滤芯 科学组合净化工艺 智能显示屏 触摸式按键 阻菌龙头 前置出水口全封闭 半集成水路设计 减少漏水风险", Name = "75C-2智能纯水机", Price = 9999, PictureFileName = "9.jpg" },
            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 1, AvailableStock = 100, Description = "高效RO膜滤芯 去除率达90%以上 五层滤芯及科学组合净化工艺 炫彩触摸式按键 独特磁吸设计机身简洁 滤芯智能冲洗 微电脑智能控制", Name = "L3纯水机", Price = 9999, PictureFileName = "10.jpg" },
            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 1, AvailableStock = 100, Description = "高效RO膜滤芯 去除率达90%以上 精确寿命监控 随时掌控滤芯状况 炫彩触摸式按键 自吸式无需水压 智能化空吸保护", Name = "L2纯水机", Price = 9999, PictureFileName = "11.jpg" },
            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 1, AvailableStock = 100, Description = "高效RO膜滤芯 去除率达90%以上 三级复合滤芯 微电脑控制 智能化保护 触摸式按键 操作更方便 独特杀菌模块 去除二次污染", Name = "GXRO80C 杀菌型智能纯水机", Price = 9999, PictureFileName = "12.jpg" },
            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 1, AvailableStock = 100, Description = "采用600GRO膜 平均8S一杯水 物理阻菌 保证最后一厘米的纯净 嵌入式漏水保护监控更准确 可选择常规模式或节水模式 过流式紫外线杀菌", Name = "L6纯水机（标准型）", Price= 9999, PictureFileName = "13.jpg" },
            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 1, AvailableStock = 100, Description = "4合1全新高集成滤芯设计高端智能按键龙头1:1超低废水比安全防伪二维码高性能紫外线杀菌技术", Name = "S1纯水机", Price = 9999, PictureFileName = "14.jpg" },

            new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 1, AvailableStock = 100, Description = "独特内胆 省电节能 智能触控 时尚科技 人性化操作 界面易懂 电子童锁实用安全 水电自动分离 停水贴心保护", Name = "GA406B 温热管线饮水机", Price = 9999, PictureFileName = "15.jpg" },
            new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 1, AvailableStock = 100, Description = "采用热胆加热方式 出水速度更快 选用优质热胆保温棉 PU材质 选用涂三防漆的PCB板 醒目的琴键式按压开关 可靠性高 可拆卸接水盒 清洗更方便", Name = "G1管线饮水机", Price = 9999, PictureFileName = "16.jpg" },
            new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 1, AvailableStock = 100, Description = "童锁 防干烧 多重安全保护 五档温度 选择不同出水温度 3秒即热杜绝反复加热 3.2L大水箱满足全天候饮水需求 旋钮调温 取水操作方便", Name = "GT3桌面即热饮水机", Price = 9999, PictureFileName = "17.jpg" },
            new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 1, AvailableStock = 100, Description = "六重智能防护 高原模式 精准控温 硅胶密封设计 休眠模式", Name = "朗诗德G3速热管线机", Price= 9999, PictureFileName = "18.jpg" },
            new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 1, AvailableStock = 100, Description = "纯净热饮，触手可得多重用水防护，守护家人安全智能灯环龙头，水温状态一目了然厨下安装更省空间，体积小更美观贴心童锁，防止烫伤高原模式，轻松设置", Name = "朗诗德牌厨下热饮机", Price = 9999, PictureFileName = "19.jpg" },

            new CatalogItem { CatalogTypeId = 4, CatalogBrandId = 1, AvailableStock = 100, Description = "纤薄机身，小巧免安装，通电即用。智能精准,定义不同饮品最佳口感。茶胶囊可多次冲泡，家人朋友聚会齐分享。胶囊结构充氮和隔氧保鲜设计，久置冲泡亦可即刻感受香醇。高原模式，系统自动调节冲泡条件，保障不同地区使用的最佳口感。", Name = "饮立方", Price= 9999, PictureFileName = "20.jpg" },
            new CatalogItem { CatalogTypeId = 4, CatalogBrandId = 1, AvailableStock = 100, Description = "复合滤芯防伪设计。三机一体，定义智饮新时代。饮品智能扫码萃取，精准定义最佳口感。胶囊结构充氮和隔氧保鲜设计，即刻感受饮品香醇。茶胶囊可多次冲泡，家人朋友聚会齐分享。", Name = "饮立方Plus", Price = 9999, PictureFileName = "21.jpg" },

            new CatalogItem { CatalogTypeId = 5, CatalogBrandId = 1, AvailableStock = 100, Description = "有效吸附钙镁离子软化水质一体式抛弃式滤芯设计天然水果提取物制成有机滤料出水孔利用工程学设计原理采用耐温ABS材质", Name = "软水花洒", Price= 9999, PictureFileName = "22.jpg" },
            new CatalogItem { CatalogTypeId = 5, CatalogBrandId = 1, AvailableStock = 100, Description = "首款具有软水功能的洗脸龙头 一体式可更换滤芯设计 便捷无污染 最外层不锈钢网，过滤大于0.01毫米的颗粒 下层KDF用于吸收水中的余氯 氯化物及重金属铅、汞 上层的柠檬提取物用于吸收水中的钙、镁离子", Name = "RL2软水龙头", Price = 9999, PictureFileName = "23.jpg" },

            new CatalogItem { CatalogTypeId = 6, CatalogBrandId = 1, AvailableStock = 100, Description = "五级净化 层层过滤 热水随取随有 使用便利 一体式设计 节省空间 加大内胆 持续供应热水", Name = "RO一体开水机（双龙头）", Price =9999, PictureFileName = "24.jpg" },
            new CatalogItem { CatalogTypeId = 6, CatalogBrandId = 1, AvailableStock = 100, Description = "超静音制水系统五级过滤制水系统高回收率高效杀菌", Name = "悦纯系列商用纯水机", Price= 9999, PictureFileName = "25.jpg" },
            new CatalogItem { CatalogTypeId = 6, CatalogBrandId = 1, AvailableStock = 100, Description = "生剂和65%的水智能自动循环运行", Name = "豪华商用纯水机", Price = 9999, PictureFileName = "26.jpg" },
            new CatalogItem { CatalogTypeId = 6, CatalogBrandId = 1, AvailableStock = 100, Description = "超静音制水系统五级过滤制水系统高回收率高效杀菌", Name = "智爱系列商用直饮机", Price = 9999, PictureFileName = "27.jpg" },
            new CatalogItem { CatalogTypeId = 6, CatalogBrandId = 1, AvailableStock = 100, Description = "超静音制水系统五级过滤制水系统高回收率高效杀菌", Name = "净雅系列商用直饮机", Price = 9999, PictureFileName = "28.jpg" },
            new CatalogItem { CatalogTypeId = 6, CatalogBrandId = 1, AvailableStock = 100, Description = "超静音制水系统五级过滤制水系统高回收率高温杀菌", Name = "智爱100商用直饮机", Price = 9999, PictureFileName = "29.jpg" },

            new CatalogItem { CatalogTypeId = 7, CatalogBrandId = 1, AvailableStock = 100, Description = "优化进风口设计 配以先进的降噪系统 内置工业级球形轴承新科技操纵 高品质静音轮 有效降低转动噪音 隐藏式收纳线 收纳无忧 每一块滤网均内置RFID芯片", Name = "智能除醛空气净化器", Price = 9999, PictureFileName = "30.jpg" },
            new CatalogItem { CatalogTypeId = 7, CatalogBrandId = 1, AvailableStock = 100, Description = "商务典藏 科技创造 澎湃动力 双倍风压", Name = "智能语音车载净化器", Price = 9999, PictureFileName = "31.jpg" },
            new CatalogItem { CatalogTypeId = 7, CatalogBrandId = 1, AvailableStock = 100, Description = "完美视觉交互设计高清LED数字显示 智能语音模块 随时掌握空气状况 甲醛专业级电化学传感器 高精度温度、湿度传感器 内置独立颗粒物激光传感器", Name = "空气质量检测仪朗朗", Price = 9999, PictureFileName = "32.jpg" },

        };
    }

    private string[] GetHeaders(string csvfile, string[] requiredHeaders, string[] optionalHeaders = null)
    {
        string[] csvheaders = File.ReadLines(csvfile).First().ToLowerInvariant().Split(',');

        if (csvheaders.Count() < requiredHeaders.Count())
        {
            throw new Exception($"requiredHeader count '{ requiredHeaders.Count()}' is bigger then csv header count '{csvheaders.Count()}' ");
        }

        if (optionalHeaders != null)
        {
            if (csvheaders.Count() > requiredHeaders.Count() + optionalHeaders.Count())
            {
                throw new Exception($"csv header count '{csvheaders.Count()}'  is larger then required '{requiredHeaders.Count()}' and optional '{optionalHeaders.Count()}' headers count");
            }
        }

        foreach (var requiredHeader in requiredHeaders)
        {
            if (!csvheaders.Contains(requiredHeader))
            {
                throw new Exception($"does not contain required header '{requiredHeader}'");
            }
        }

        return csvheaders;
    }

    private void GetCatalogItemPictures(string contentRootPath, string picturePath)
    {
        if (picturePath != null)
        {
            DirectoryInfo directory = new DirectoryInfo(picturePath);
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            string zipFileCatalogItemPictures = Path.Combine(contentRootPath, "Setup", "CatalogItems.zip");
            ZipFile.ExtractToDirectory(zipFileCatalogItemPictures, picturePath);
        }
    }
}
