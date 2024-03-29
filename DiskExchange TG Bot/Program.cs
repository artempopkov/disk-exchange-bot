﻿// 1.6.1
using Pastel;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.InlineQueryResults;
using System.Threading;

namespace DiskExchange_TG_Bot
{
    internal class Program
    {
        private static Logger log = new Logger();
        private static Database db = new Database();
        private enum awaitInfoType : int
        {
            none = 0,
            photo = 1,
            name = 2,
            price = 3,
            exchange = 4,
            location = 5,
            discNumber = 6,
            searchResult = 7,
            favNumber = 8,
            condition = 9
        };

        private static ITelegramBotClient bot;

        private static void Main(string[] args)
        {
            bot = new TelegramBotClient("token") { Timeout = TimeSpan.FromSeconds(100) };
            bot.OnMessage += Bot_OnMessage;
            bot.OnCallbackQuery += Bot_OnCallbackQuery;
            bot.OnInlineQuery += Bot_OnInlineQuery;
            Console.Write($"2/2: Starting @discExchangeBot... ".Pastel(Color.Yellow));
            Console.Beep();
            Thread song = new Thread(PlaySounds);
            song.Start();
            try
            {
                bot.StartReceiving();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nStartup failed! Error message: " + e.Message.Pastel(Color.Red));
                Console.ReadKey();
                return;
            }

            Console.WriteLine("[READY]\n");
            Console.WriteLine(">ALL SYSTEMS READY\n>Welcome, admin\n");
            Console.CursorVisible = false;

            Console.ReadLine();
        }
        static void PlaySounds()
        {
            while (true)
            {
                refrenSolo();
                coupleSolo();
                refrenSolo();
            }
        }
        static void refrenSolo()
        {
            Console.Beep(659, 300);
            Console.Beep(659, 300);
            Console.Beep(659, 300);
            Thread.Sleep(300);
            Console.Beep(659, 300);
            Console.Beep(659, 300);
            Console.Beep(659, 300);
            Thread.Sleep(300);
            Console.Beep(659, 300);
            Console.Beep(783, 300);
            Console.Beep(523, 300);
            Console.Beep(587, 300);
            Console.Beep(659, 300);
            Console.Beep(261, 300);
            Console.Beep(293, 300);
            Console.Beep(329, 300);
            Console.Beep(698, 300);
            Console.Beep(698, 300);
            Console.Beep(698, 300);
            Thread.Sleep(300);
            Console.Beep(698, 300);
            Console.Beep(659, 300);
            Console.Beep(659, 300);
            Thread.Sleep(300);
            Console.Beep(659, 300);
            Console.Beep(587, 300);
            Console.Beep(587, 300);
            Console.Beep(659, 300);
            Console.Beep(587, 300);
            Thread.Sleep(300);
            Console.Beep(783, 300);
            Thread.Sleep(300);
            Console.Beep(659, 300);
            Console.Beep(659, 300);
            Console.Beep(659, 300);
            Thread.Sleep(300);
            Console.Beep(659, 300);
            Console.Beep(659, 300);
            Console.Beep(659, 300);
            Thread.Sleep(300);
            Console.Beep(659, 300);
            Console.Beep(783, 300);
            Console.Beep(523, 300);
            Console.Beep(587, 300);
            Console.Beep(659, 300);
            Console.Beep(261, 300);
            Console.Beep(293, 300);
            Console.Beep(329, 300);
            Console.Beep(698, 300);
            Console.Beep(698, 300);
            Console.Beep(698, 300);
            Thread.Sleep(300);
            Console.Beep(698, 300);
            Console.Beep(659, 300);
            Console.Beep(659, 300);
            Thread.Sleep(300);
            Console.Beep(783, 300);
            Console.Beep(783, 300);
            Console.Beep(698, 300);
            Console.Beep(587, 300);
            Console.Beep(523, 600);
            Thread.Sleep(600);
        }
        static void coupleSolo()
        {
            Console.Beep(392, 300);
            Console.Beep(659, 300);
            Console.Beep(587, 300);
            Console.Beep(523, 300);
            Console.Beep(392, 600);
            Thread.Sleep(300 * 2);
            Console.Beep(392, 300);
            Console.Beep(659, 300);
            Console.Beep(587, 300);
            Console.Beep(523, 300);
            Console.Beep(440, 600);
            Thread.Sleep(600);
            Console.Beep(440, 300);
            Console.Beep(698, 300);
            Console.Beep(659, 300);
            Console.Beep(587, 300);
            Console.Beep(783, 600);
            Thread.Sleep(600);
            Console.Beep(880, 300);
            Console.Beep(880, 300);
            Console.Beep(783, 300);
            Console.Beep(622, 300);
            Console.Beep(659, 600);
            Thread.Sleep(600);
            Console.Beep(392, 300);
            Console.Beep(659, 300);
            Console.Beep(587, 300);
            Console.Beep(523, 300);
            Console.Beep(392, 600);
            Thread.Sleep(600);
            Console.Beep(392, 300);
            Console.Beep(659, 300);
            Console.Beep(587, 300);
            Console.Beep(523, 300);
            Console.Beep(440, 600);
            Thread.Sleep(600);
            Console.Beep(440, 300);
            Console.Beep(698, 300);
            Console.Beep(659, 300);
            Console.Beep(587, 300);
            Console.Beep(783, 600);
            Thread.Sleep(600);
            Console.Beep(880, 300);
            Console.Beep(783, 300);
            Console.Beep(698, 300);
            Console.Beep(587, 300);
            Console.Beep(523, 600);
            Thread.Sleep(600);
        }

        private static async void Bot_OnInlineQuery(object sender, Telegram.Bot.Args.InlineQueryEventArgs e)
        {
            Database.discsArray[] discs = null;
            string query = e.InlineQuery.Query;
            if (query.Length < 1)
            {
                discs = db.GetDiscsArrays();
            }
            else
            {
                discs = db.Search(query.ToLower());
            }

            InlineQueryResultBase[] results = new InlineQueryResultArticle[discs.Length];
            for (int i = 0; i < results.Length; i++)
            {
                var temp = new InlineQueryResultArticle(Convert.ToString(discs[i].id),
                    title: discs[i].name,
                    new InputTextMessageContent($"Товар {discs[i].id}: {discs[i].name}"));
                temp.Description = discs[i].platform + " | " + discs[i].price + " BYN";
                results[i] = temp;
            }
            if (results.Length == 0)
                return;
            Console.WriteLine(query);
            db.SetAwaitInfoType(e.InlineQuery.From.Id, (int)awaitInfoType.searchResult);
            try
            {
                await bot.AnswerInlineQueryAsync(e.InlineQuery.Id, results);
            }
            catch (Telegram.Bot.Exceptions.InvalidParameterException e5)
            {
                log.Error(e5.Message);
                return;
            }
        }
        private static async void Bot_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var data = e.CallbackQuery.Data;
            var message = e.CallbackQuery.Message;
            log.Query(e);

            try
            {
                switch (data)
                {
                    default:
                        return;
                    case "🛒 Связаться с продавцом 🛒":
                        string seller = db.GetUserPhone(e.CallbackQuery.From.Id);
                        if (seller[0] == '+')
                            await bot.SendContactAsync(e.CallbackQuery.From.Id, seller, "Продавец");
                        else
                            await bot.SendTextMessageAsync(e.CallbackQuery.From.Id, '@' + seller);
                        return;

                    case "⭐️ В избранное ⭐️":
                        db.AddSelectedOfferToFavorites(e.CallbackQuery.From.Id);
                        await bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                            "Товар добален в избранное", true);
                        return;
                    case "PS4 ⚪️":
                        db.SetPlatform(0, e.CallbackQuery.From.Id, true);
                        break;

                    case "Xbox ⚪️":
                        db.SetPlatform(1, e.CallbackQuery.From.Id, true);
                        break;

                    case "Switch ⚪️":
                        db.SetPlatform(2, e.CallbackQuery.From.Id, true);
                        break;

                    case "Изменить название":
                        await bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                            "Отправьте название игры в следующем сообщении.", true);
                        db.SetAwaitInfoType(e.CallbackQuery.From.Id, (int)awaitInfoType.name);
                        return;

                    case "Указать цену":
                        await bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                            "Отправьте цену игры в следующем сообщении.", true);
                        db.SetAwaitInfoType(e.CallbackQuery.From.Id, (int)awaitInfoType.price);
                        return;
                    case "Состояние":
                        await bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                           "Отправьте состояние диска в следующем сообщении", true);
                        db.SetAwaitInfoType(e.CallbackQuery.From.Id, (int)awaitInfoType.condition);
                        return;


                    case "Обмен":
                        if (db.GetExchange(e.CallbackQuery.From.Id, true) != "")
                        {
                            db.SetExchange("", e.CallbackQuery.From.Id, true);
                            break;
                        }
                        else
                        {
                            await bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                            "Отправьте названия желаемых игр в следующем сообщении.", true);
                            db.SetAwaitInfoType(e.CallbackQuery.From.Id, (int)awaitInfoType.exchange);
                            return;
                        }
                    case "Загрузить фото":
                        await bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                            "Отправьте фотогрфию в следующем сообщении.", true);
                        db.SetAwaitInfoType(e.CallbackQuery.From.Id, (int)awaitInfoType.photo);
                        return;

                    case "✅ Сохранить ✅":
                        await bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, db.GetEditMessageId(e.CallbackQuery.From.Id));
                        await bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "✅ Товар добавлен!\n\nℹ️ Чтобы просмотреть список ваших товаров, выберите пункт \"Мои товары\".");
                        return;

                    case "❌ Удалить ❌":
                        db.DeleteOffer(e.CallbackQuery.From.Id);
                        await bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, db.GetEditMessageId(e.CallbackQuery.From.Id));
                        await bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "✅ Товар удален!\n\nℹ️ Чтобы просмотреть список ваших товаров, выберите пункт \"Мои товары\".");
                        return;
                    case "❌ Удалить из избранного ❌":
                        db.DeleteOfferFromFav(db.GetEditOfferId(e.CallbackQuery.From.Id));
                        await bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, db.GetEditMessageId(e.CallbackQuery.From.Id));
                        await bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "✅ Товар удален из избранного!\n\nℹ️ Чтобы просмотреть список избранного, выберите пункт \"Избранное\".");
                        return;
                }
                await bot.EditMessageCaptionAsync(message.Chat.Id,
                message.MessageId,
                caption: db.GetCaption(e.CallbackQuery.From.Id, true),
                replyMarkup: IReplies.editKeyboard(db.GetOfferPlatform(e.CallbackQuery.From.Id)));
            }
            catch (Exception e3)
            {
                log.Error(e3.Message);
                return;
            }
        }
        private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            // return;
            var text = e.Message.Text;
            var message = e.Message;
            log.Message(e);
            //if (e.Message.From.Id == 652761067)
            //    return;
            try
            {
                if (message.Type == Telegram.Bot.Types.Enums.MessageType.Contact)
                    db.SetUserPhone(message.From.Id, message.Contact.PhoneNumber);
                switch (message.Text)
                {
                    default:
                        switch (db.GetAwaitInfoType(message.From.Id))
                        {
                            case 0:
                                break;

                            case (int)awaitInfoType.name:
                                db.SetName(text, message.From.Id, true);
                                db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.none);
                                await SetDiscCaptionAsync(message.Chat.Id, message.From.Id);
                                break;

                            case (int)awaitInfoType.price:
                                if (!(message.Text.Length == message.Text.Where(c => char.IsDigit(c)).Count()))
                                {
                                    await bot.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                                    await bot.SendTextMessageAsync(message.From.Id, $"Цена должна состоять только из цифр!");
                                    return;
                                }
                                if (message.Text.Length > 4)
                                {
                                    await bot.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                                    await bot.SendTextMessageAsync(message.From.Id, $"Цена должна быть не более 9999!");
                                    return;
                                }

                                db.SetPrice(message.Text, message.From.Id, true);
                                db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.none);
                                await SetDiscCaptionAsync(message.Chat.Id, message.From.Id);
                                break;

                            case (int)awaitInfoType.exchange:
                                db.SetExchange(text, message.From.Id, true);
                                db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.none);
                                await SetDiscCaptionAsync(message.Chat.Id, message.From.Id);
                                break;

                            case (int)awaitInfoType.location:
                                db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.none);
                                db.SetLocation(text, message.From.Id);
                                await bot.SendTextMessageAsync(message.From.Id, $"Профиль создан.",
                                    replyMarkup: IReplies.keyboards.main);
                                Console.WriteLine();
                                return;

                            case (int)awaitInfoType.photo:
                                if (message.Photo == null)
                                    break;
                                string photo = message.Photo[message.Photo.Length - 1].FileId;
                                db.SetPhoto(photo, message.From.Id, true);
                                await bot.EditMessageMediaAsync(
                                    chatId: message.Chat.Id,
                                    messageId: db.GetEditMessageId(message.From.Id),
                                    media: new Telegram.Bot.Types.InputMediaPhoto(photo));
                                await SetDiscCaptionAsync(message.Chat.Id, message.From.Id);
                                break;

                            case (int)awaitInfoType.discNumber:
                                db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.none);
                                var temp1 = await bot.SendPhotoAsync(message.Chat.Id, db.GetPhotoForList(message.From.Id, Convert.ToInt32(message.Text)),
                                    caption: db.GetSelectedFromListOffer(message.From.Id, Convert.ToInt32(message.Text)),
                                    replyMarkup: IReplies.editKeyboard(db.GetOfferPlatform(message.From.Id)));
                                db.SetEditMessageId(message.From.Id, temp1.MessageId);
                                break;

                            case (int)awaitInfoType.searchResult:
                                db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.none);
                                int discId = Convert.ToInt32(message.Text.Substring(5).Split(':')[0]);
                                db.SetSelectedOffer(message.From.Id, discId);
                                await bot.SendPhotoAsync(message.Chat.Id, db.GetPhoto(discId),
                                    caption: db.GetCaption(discId),
                                    replyMarkup: IReplies.discKeyboard());
                                break;
                            case (int)awaitInfoType.favNumber:
                                if (Convert.ToInt32(message.Text) > db.GetAmountOfFav(message.From.Id) || Convert.ToInt32(message.Text) < 1)
                                    break;

                                db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.none);
                                int ownerId = db.GetOwnerId(db.GetSelectedOffer(message.From.Id));
                                var temp2 = await bot.SendPhotoAsync(message.Chat.Id, db.GetPhoto(db.GetFavDisc(message.From.Id, Convert.ToInt32(message.Text))),
                                    caption: db.GetSelectedFromFav(db.GetFavDisc(message.From.Id, Convert.ToInt32(message.Text))),
                                    replyMarkup: IReplies.favKeyboard());
                                db.SetEditMessageId(message.From.Id, temp2.MessageId);
                                db.SetEditOfferId(message.From.Id, db.GetFavDisc(message.From.Id, Convert.ToInt32(message.Text)));
                                break;
                            case (int)awaitInfoType.condition:
                                if (message.Text.Length > 15 || message.Text.Length < 3)
                                {
                                    await bot.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                                    await bot.SendTextMessageAsync(message.From.Id, $"Длинна строки должна быть от 3 до 15 символов!");
                                }
                                db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.none);
                                db.SetCondition(text, message.From.Id, true);
                                await SetDiscCaptionAsync(message.Chat.Id, message.From.Id);
                                break;
                            default:
                                Console.WriteLine("Unprocessed message found. Deleted.".Pastel(Color.Gold));
                                break;
                        }
                        await bot.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);// удаление сообщений
                        return;

                    case "/start":
                        db.NewUser(message.From.Id, message.From.Username);
                        await bot.SendTextMessageAsync(message.From.Id, $"Привет {message.From.Username}, это бот по обмену дисками!");
                        await bot.SendTextMessageAsync(message.From.Id, $"Пожалуйста, введите ваш город проживания:");
                        db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.location);
                        break;

                    case "/keyboard":
                        await bot.SendTextMessageAsync(message.Chat.Id, "Выберите опцию из меню ниже:",
                            replyMarkup: IReplies.keyboards.main);
                        break;

                    case "Назад ↩️":
                        await bot.SendTextMessageAsync(message.Chat.Id, "Выберите опцию из меню ниже:",
                            replyMarkup: IReplies.keyboards.main);
                        break;

                    case "Контакты 📱":
                        await bot.SendTextMessageAsync(message.Chat.Id, "Выберите опцию из меню ниже:",
                            replyMarkup: IReplies.keyboards.contact);
                        break;
                    case "Cправка":
                        await bot.SendTextMessageAsync(message.Chat.Id, "'Добавить товар' - в этом пункте пользователь может добавть новое объявление\n\n'Мои товары' - при наличии пользоваель может редактировать или удалять свои товары\n\n'Поиск' - поиск объявлений по наименованию диска\n\n 'Избранное' - после добавления товара в избранное, он будет отображатся в данном пункте меню\n\n 'Связатся с продавцом' - при нажатии будет предоставлена информация для связи с продавцом",
                            replyMarkup: IReplies.keyboards.help);
                        break;
                    case "Помощь ❓":
                        await bot.SendTextMessageAsync(message.Chat.Id, "Выберите опцию из меню ниже:",
                            replyMarkup: IReplies.keyboards.help);
                        break;

                    case "Поиск 🔎":
                        await bot.SendTextMessageAsync(message.Chat.Id, "Чтобы начать поиск игр, нажмите на кнопку ниже:",
                            replyMarkup: IReplies.keyboards.search);
                        db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.searchResult);
                        break;

                    case "Мой профиль 👤":
                        await bot.SendTextMessageAsync(message.Chat.Id, "Выберите опцию из меню ниже:",
                            replyMarkup: IReplies.keyboards.profile);
                        break;

                    case "Мои товары 💿":
                        if (db.UserHasOffers(message.From.Id))
                        {
                            await bot.SendTextMessageAsync(message.Chat.Id, db.GetUserOffers(message.From.Id));
                            db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.discNumber);
                        }
                        else
                        {
                            await bot.SendTextMessageAsync(message.Chat.Id, "У вас нет созданных дисков:",
                            replyMarkup: IReplies.keyboards.profile);
                            db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.none);
                        }
                        break;

                    case "Добавить товар 💿":
                        if (e.Message.From.Username == null && db.GetUserPhoneFirstCheck(message.From.Id) == "")
                        {
                            await bot.SendTextMessageAsync(e.Message.Chat.Id, "ℹ️ Не удалось получить ваш никнейм.\n\n" +
                                "Чтобы покупатель мог связатся с вами, добавьте свой номер телефона в настройках профиля.\n" +
                                "Вы также можете создать свой никнейм и повторить попытку.");
                            return;
                        }
                        db.NewOffer(message.From.Id);
                        var temp = await bot.SendPhotoAsync(message.Chat.Id, "AgACAgIAAxkBAAIGZF9aSti3CZNeKoW3AjRGDco3-45KAAL3rjEb0L7RSjbSrDV25SE0ECFzly4AAwEAAwIAA3gAA3CNAAIbBA",
                            caption: db.GetCaption(message.From.Id, true),
                            replyMarkup: IReplies.editKeyboard(db.GetOfferPlatform(message.From.Id)));
                        db.SetEditMessageId(message.From.Id, temp.MessageId);
                        break;

                    case "Избранное 🌟":
                        if (db.UserHasFav(message.From.Id))
                        {
                            await bot.SendTextMessageAsync(message.Chat.Id, db.GetUserFav(message.From.Id));
                            db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.favNumber);
                        }
                        else
                        {
                            db.SetAwaitInfoType(message.From.Id, (int)awaitInfoType.none);
                            await bot.SendTextMessageAsync(message.Chat.Id, "У вас нет избранных товаров",
                            replyMarkup: IReplies.keyboards.main);
                        }
                        break;
                }
            }
            catch (MessageIsNotModifiedException e1)
            {
                log.Error(e1.Message);
                return;
            }
            catch (FormatException e2)
            {
                log.Error(e2.Message);
                return;
            }
            catch (ApiRequestException e4)
            {
                log.Error(e4.Message);
                return;
            }
            catch (System.Net.Http.HttpRequestException e3)
            {
                log.Error(e3.Message);
                await bot.GetUpdatesAsync();
                return;
            }
        }
        private static async Task SetDiscCaptionAsync(long chat, int from)
        {
            await bot.EditMessageCaptionAsync(chat,
                db.GetEditMessageId(from),
                caption: db.GetCaption(from, true),
                replyMarkup: IReplies.editKeyboard(db.GetOfferPlatform(from)));
        }
    }
}
